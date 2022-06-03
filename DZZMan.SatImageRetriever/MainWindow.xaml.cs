using CopernicusAPI;
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

namespace DZZMan.SatImageRetriever
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CopernicusApi api = new();
        private DateTime date = new DateTime(2021, 10, 18);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTB.Text) || string.IsNullOrEmpty(PasswordTB.Password))
            {
                MessageBox.Show("Не все поля заполнены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //if (api.LoginAsync(LoginTB.Text, PasswordTB.Password).Result)
            if (true)
            {
                SatelliteGroupBox.IsEnabled = true;
                AuthorizationGroupBox.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Не удалось авторизоваться. Проверьте правильность логина/пароля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }    
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(LatitudeTB.Text, out double Latitude))
            {
                MessageBox.Show("Latitude wrong format", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (double.TryParse(LongitudeTB.Text, out double Longitude))
            {
                MessageBox.Show("Longitude wrong format", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            const double dx = 0.012192 * 10,
                         dy = 0.04948 * 10;

            var productCollection = api.GetProductsAsync(
                SatelliteNameTB.Text, 
                new double[][]
                {
                    new double[] { Longitude - 0.604444 - dy , Latitude + 1.207223 - dx, 1},
                    new double[] { Longitude - 0.156111 + dy , Latitude + 1.318056 + dx, 2},
                    new double[] { Longitude + 0.555833 + dy , Latitude - 1.181666 + dx, 3},
                    new double[] { Longitude + 0.122778 - dy , Latitude - 1.286111 - dx, 4},
                    new double[] { Longitude - 0.604444 - dy , Latitude + 1.207223 - dx, 5},
                }, 
                date);

            Console.WriteLine(productCollection);
        }

        private void ChooseDateTime_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
