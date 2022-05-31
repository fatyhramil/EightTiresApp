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
using System.Windows.Shapes;
using EightTiresApp.Models;

namespace EightTiresApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для PriceChangeModalWindow.xaml
    /// </summary>
    public partial class PriceChangeModalWindow : Window
    {
        List<Product> localProducts;
        public PriceChangeModalWindow(List<Product> products)
        {
            InitializeComponent();
            localProducts = products;
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (priceBox.Text != "")
                {
                    int price = Convert.ToInt32(priceBox.Text);
                    foreach (Product product in localProducts)
                    {
                        product.MinCostForAgent += price;
                    }
                    MainWindow.ent.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Введено пустое значение");
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: {0}", ex.Message);
            }
        }

        public string NewPrice
        {
            get 
            { 
                return priceBox.Text;
            }
        }

        private void priceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"\D");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
