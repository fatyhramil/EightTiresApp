using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using EightTiresApp.Models;
using Microsoft.Win32;

namespace EightTiresApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        Product localProduct;
        bool _isEdit=false;
        public AddEditProductPage(Product product, bool isEdit)
        {
            try
            {


                InitializeComponent();
                localProduct = product;
                DataContext = localProduct;
                _isEdit = isEdit;

                if (!_isEdit)
                {
                    ProductMaterialList.Height = 0;

                    AddProductMaterialBtn.Height = 0;
                    DeleteProductMaterialBtn.Height = 0;
                    DeleteBtn.Height = 0;
                }
                else
                {
                    ProductMaterialList.ItemsSource = localProduct.ProductMaterial.Where(c => c.IsDeleted != true).ToList();
                }

                ProductTypeCB.ItemsSource = MainWindow.ent.ProductType.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductListPage());
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (Validation())
                {
                    if (!_isEdit)
                    {
                        MainWindow.ent.Product.Add(localProduct);
                    }
                    MainWindow.ent.SaveChanges();
                    NavigationService.Navigate(new ProductListPage());
                }
                else
                {
                    MessageBox.Show("Test");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var result = MessageBox.Show("Вы хотите удалить продукт?", "Удаление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (localProduct.ProductSale.Count > 0)
                    {
                        MessageBox.Show("Нельзя удалить данный продукт, т.к. у него есть история продаж");
                    }
                    else
                    {
                        if (localProduct.ProductMaterial.Count > 0)
                        {
                            foreach (ProductMaterial productMaterial in localProduct.ProductMaterial)
                            {
                                productMaterial.IsDeleted = true;
                            }
                        }
                        if (localProduct.ProductCostHistory.Count > 0)
                        {
                            foreach (ProductCostHistory productCostHistory in localProduct.ProductCostHistory)
                            {
                                productCostHistory.IsDeleted = true;
                            }
                        }

                        localProduct.IsDeleted = true;
                        MainWindow.ent.SaveChanges();
                        NavigationService.Navigate(new ProductListPage());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void AddProductMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isEdit)
            {
                NavigationService.Navigate(new AddProductMaterialPage(localProduct));
            }
          
        }

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;"
                };
                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    ProductImage.Source = new BitmapImage(new Uri(dialog.FileName));
                    localProduct.Image = @"\products\" + System.IO.Path.GetFileName(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}",ex.Message);
            }
        }

        private void DeleteProductMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductMaterial pm = ProductMaterialList.SelectedItem as ProductMaterial;
                if (pm != null)
                {
                    var result = MessageBox.Show("Вы хотите удалить материал?", "Удаление", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        ProductMaterial productMaterial = localProduct.ProductMaterial.Where(c => c.MaterialID == pm.MaterialID && c.ProductID == pm.ProductID).FirstOrDefault();
                        if (productMaterial != null)
                        {
                            productMaterial.IsDeleted = true;
                            MainWindow.ent.SaveChanges();
                            ProductMaterialList.ItemsSource = localProduct.ProductMaterial.Where(c => c.IsDeleted != true).ToList();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали материал");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }


        //Проверка полей на соответствие требоаниям
        private bool Validation()
        {
            try
            {
                if (TitleTB.Text == "")
                {
                    MessageBox.Show("Не введено название продукта!");
                    return false;
                }
                if (ArticleTB.Text == "")
                {
                    MessageBox.Show("Не введен артикул!");
                    return false;
                }
                if (WarehouseCountTB.Text == "")
                {
                    MessageBox.Show("Не введено кол-во людей для производства!");
                    return false;
                }
                if (WarehouseNumberTB.Text == "")
                {
                    MessageBox.Show("Не введен номер цеха!");
                    return false;
                }
                if (MinCostcTB.Text == "")
                {
                    MessageBox.Show("Не введена минимальная стоимость для агента!");
                    return false;
                }
                if (ProductTypeCB.SelectedItem == null)
                {
                    MessageBox.Show("Не введен тип продукта!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
                return false;
            }
        }

        private void IntTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"\D");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void StringTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"\d");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
