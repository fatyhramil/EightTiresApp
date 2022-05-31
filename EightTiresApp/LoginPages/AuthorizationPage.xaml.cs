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

namespace EightTiresApp.LoginPages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        Random random = new Random();
        string kaptcha = "";
        public AuthorizationPage()
        {
            InitializeComponent();
            GenerateSymbol(4);
            GenerateNoise(10);
        }

        private async void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (KaptchaTB.Text != ""&&LoginTB.Text!=""&&PasswordTB.Text!="")
            {
                if(KaptchaTB.Text == kaptcha)
                {
                    var user = MainWindow.ent.Agent.Where(c => c.Login == LoginTB.Text).First();
                    if (user != null)
                    {
                        if (user.Password == PasswordTB.Text)
                        {
                            NavigationService.Navigate(new ProductListPage(user));
                        }
                        else
                        {
                            MessageBox.Show("Неправильный пароль!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователя с таким логином не существует!");
                    }
                }
                else
                {
                    
                    await PutTaskDelay();
                }
            }
            else
            {
                MessageBox.Show("Не все значения введены");
            }
        }
        async Task PutTaskDelay()
        {
            MessageBox.Show("Неправильно введена капча");
            KaptchaTB.IsEnabled = false;
            EnterBtn.IsEnabled = false;
            KaptchaRefreshBtn.IsEnabled = false;
            await Task.Delay(10000);
            KaptchaTB.IsEnabled = true;
            EnterBtn.IsEnabled = true;
            KaptchaRefreshBtn.IsEnabled = true;
        }


        private void GenerateSymbol(int count)
        {
            string alp = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            for (int i = 0; i < count; i++)
            {
                char symbol = alp.ElementAt(random.Next(0, alp.Length));
                kaptcha += symbol;
                TextBlock tb = new TextBlock();
                tb.Text = symbol.ToString();
                tb.FontSize = random.Next(20, 60);
                //tb.RenderTransform = new RotateTransform(random.Next(-45, 45));
                tb.Margin = new Thickness(10, 0, 10, 0);
                tb.TextDecorations = TextDecorations.Strikethrough;
                Symbols.Children.Add(tb);
            }
        }

        private void GenerateNoise(int noise)
        {
            for (int i = 0; i < noise; i++)
            {

                //Ellipse ellipse = new Ellipse();
                //ellipse.Fill = new SolidColorBrush(Color.FromArgb((byte)random.Next(10, 200),
                //    (byte)random.Next(0, 256),
                //    (byte)random.Next(0, 256),
                //    (byte)random.Next(0, 256)));
                //ellipse.Height = random.Next(20, 50);
                //ellipse.Width = random.Next(20, 50);
                //CanvasN.Children.Add(ellipse);
                //Canvas.SetLeft(ellipse, random.Next(0, 100));
                //Canvas.SetTop(ellipse, random.Next(0, 40));
            }
        }

        private void KaptchaRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            CanvasN.Children.Clear();
            Symbols.Children.Clear();
            kaptcha = "";
            GenerateSymbol(4);
            GenerateNoise(10);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductListPage());
        }
    }
}
