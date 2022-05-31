using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EightTiresApp.LoginPages;
using EightTiresApp.Models;

namespace EightTiresApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        List<ProductType> productTypes = new List<ProductType>();
        public List<Product> products = new List<Product>();
        List<Product> selectedProducts = new List<Product>();
        int take = 20;
        int skip = 0;
        private readonly List<Button> buttons = new List<Button>();
        public Agent localAgent;
        public ProductListPage()
        {
            try
            {
                InitializeComponent();
                products = MainWindow.ent.Product.Where(c => c.IsDeleted != true).ToList();
                productTypes = MainWindow.ent.ProductType.ToList();
                productTypes.Insert(0, new ProductType { Title = "Все типы" });
                FIlterCB.ItemsSource = productTypes;
                ProductList.ItemsSource = products.Take(take);
                PreviousBtn.Content = "<";
                NextBtn.Content = ">";
                GenerateNavigationButtons();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }
        public ProductListPage(Agent agent)
        {
            try
            {
                InitializeComponent();
                localAgent = agent;
                AuthorizedUserNameTB.Visibility = Visibility.Visible;
                AuthorizationBtn.Visibility = Visibility.Visible;
                AuthorizationBtn.Height = 0;
                AuthorizedUserNameTB.Text = localAgent.Login;
                products = MainWindow.ent.Product.Where(c => c.IsDeleted != true).ToList();
                productTypes = MainWindow.ent.ProductType.ToList();
                productTypes.Insert(0, new ProductType { Title = "Все типы" });
                FIlterCB.ItemsSource = productTypes;
                ProductList.ItemsSource = products.Take(take);
                PreviousBtn.Content = "<";
                NextBtn.Content = ">";
                GenerateNavigationButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }
        private void GenerateNavigationButtons()
        {
            try
            {
                int count;
                if (products.Count % take == 0)
                    count = products.Count / take;
                else
                {
                    count = (products.Count / take) + 1;
                }

                for (int i = 1; i <= count; i++)
                {
                    Button button = new Button
                    {
                        Content = i,
                        BorderBrush = new SolidColorBrush(Colors.White),
                        Background = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(5),
                        Width = 25,
                        Height = 25
                    };
                    button.Click += new RoutedEventHandler(GeneratedButton_Click);
                    buttons.Add(button);
                    ButtonsStack.Children.Add(button);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }
        private void GeneratedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int number = (int)(sender as Button).Content;
                skip = take * (number - 1);
                ProductList.ItemsSource = products.Skip(skip).Take(take);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int checkSkip = skip - take;
                if (checkSkip >= 0)
                {
                    skip -= take;
                    ProductList.ItemsSource = products.Skip(skip).Take(take);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int checkSkip = skip + take;

                if (checkSkip < products.Count - take)
                {
                    skip += take;
                    ProductList.ItemsSource = products.Skip(skip).Take(take);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Фильтрация списка продуктов
        /// </summary>
        void Filter()
        {
            try
            {
                products = MainWindow.ent.Product.ToList();
                if (SearchNameDescriptionTB.Text != "")
                {
                    products = products.Where(c => c.Title.StartsWith(SearchNameDescriptionTB.Text)).Take(take).ToList();
                }
                if (FIlterCB.SelectedItem != null)
                {
                    ProductType productType = FIlterCB.SelectedItem as ProductType;
                    if (productType != null)
                    {
                        if (productType.Title != "Все типы")
                        {
                            products = products.Where(c => c.ProductType.Title == productType.Title).Take(take).ToList();
                        }
                        else
                        {
                            products = products.ToList();
                        }
                    }
                }
                ProductList.ItemsSource = products.Take(take);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Вызов метода фильтрации при изменении Выпадающего списка
        /// </summary>
        private void FIlterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Сортировка списка продуктов
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                if (SortCB.SelectedItem != null)
                {
                    string text = (SortCB.SelectedItem as ComboBoxItem).Content as string;
                    if (text != null && text != "")
                    {
                        if (text == "От А до Я")
                        {
                            ProductList.ItemsSource = products.OrderBy(c => c.Title).Take(take).ToList();
                        }
                        if (text == "От Я до А")
                        {
                            ProductList.ItemsSource = products.OrderByDescending(c => c.Title).ToList();
                        }
                        if (text == "Номер цеха по возрастанию")
                        {
                            ProductList.ItemsSource = products.OrderBy(c => c.ProductionWorkshopNumber).ToList();
                        }
                        if (text == "Номер цеха по убыванию")
                        {
                            ProductList.ItemsSource = products.OrderByDescending(c => c.ProductionWorkshopNumber).ToList();
                        }
                        if (text == "Стоимость для агента по возрастанию")
                        {
                            ProductList.ItemsSource = products.OrderBy(c => c.MinCostForAgent).ToList();
                        }
                        if (text == "Стоимость для агента по убыванию")
                        {
                            ProductList.ItemsSource = products.OrderByDescending(c => c.MinCostForAgent).ToList();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }


        /// <summary>
        /// Вызов метода фильтрации при изменении поля с вводом названия продукта
        /// </summary>
        private void SearchNameDescriptionTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
        /// <summary>
        /// Выбор продукта для изменения
        /// </summary>
        private void ProductList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Product product = ProductList.SelectedItem as Product;
            if (product != null)
            {
                NavigationService.Navigate(new AddEditProductPage(product, true));
            }
        }
        /// <summary>
        /// Добавление нового продукта
        /// </summary>
        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(new Product(), false));
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = ProductList.SelectedItem as Product;
            if (product != null)
            {
                selectedProducts.Add(product);
            }
            if(selectedProducts.Count > 0)
            {
                ChangePriceBtn.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Открытие модального окна для изменения цены
        /// </summary>
        private void ChangePriceBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PriceChangeModalWindow window = new PriceChangeModalWindow(selectedProducts);
                if (window.ShowDialog() == true)
                {
                    if (window.NewPrice != "")
                    {
                        MessageBox.Show("Минимальная цена для агента увеличена на " + window.NewPrice.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Введено пустое значение");
                    }
                }
                selectedProducts.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void AuthorizationBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
