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
using EightTiresApp.Pages;
using EightTiresApp.Models;

namespace EightTiresApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static EightTiresDBEntities1 ent = new EightTiresDBEntities1();
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new ProductListPage());
        }
    }
}
