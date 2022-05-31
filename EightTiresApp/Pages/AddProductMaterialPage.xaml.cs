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

namespace EightTiresApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductMaterialPage.xaml
    /// </summary>
    public partial class AddProductMaterialPage : Page
    {
        Product localProduct;
        public AddProductMaterialPage(Product product)
        {
            InitializeComponent();
            localProduct = product;
            DataContext = localProduct;
            MaterialCB.ItemsSource = MainWindow.ent.Material.Where(c => c.IsDeleted != true).ToList();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(localProduct, true));
        }
        //Добавление материалов к продукту
        private void AddProductMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var material = MaterialCB.SelectedItem as Material;
                if (material != null)
                {
                    if (CountTB.Text != "")
                    {
                        int count = Convert.ToInt32(CountTB.Text);
                        ProductMaterial pm = new ProductMaterial()
                        {
                            Material = material,
                            Count = count,
                        };
                        localProduct.ProductMaterial.Add(pm);
                        MainWindow.ent.SaveChanges();
                        NavigationService.Navigate(new AddEditProductPage(localProduct, true));
                    }
                    else
                    {
                        MessageBox.Show("Вы не ввели кол-во материала");
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали материал");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        private void CountTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"\D");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
