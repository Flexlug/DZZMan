using DZZMan.API;
using DZZMan.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DZZMan.Backend.AdminClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DZZManApi api = null;
        private ObservableCollection<Satellite> Satellites { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            SatellitesList.ItemsSource = Satellites;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            api = new(IPTB.Text);
            try
            {
                api.Login(TokenInput.Password);
            }
            catch (AggregateException)
            {
                MessageBox.Show("Неверный токен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainContent.IsEnabled = true;
            Update();
        }

        private void Update()
        {
            var satellites = api.GetSatellites();

            Satellites = new(satellites);
            SatellitesList.ItemsSource = Satellites;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            MainContent.IsEnabled = false;
            Update();
            MainContent.IsEnabled = true;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainContent.IsEnabled = false;
            Satellite satellite = (sender as ListBoxItem).DataContext as Satellite;

            var editor = new SatEditor(satellite);
            var res = editor.ShowDialog();
            if (res == true)
            {
                satellite = editor.Satellite;
                try
                {
                    api.UpdateSatellite(satellite);
                    Update();
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка при обновлении данных: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            MainContent.IsEnabled = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MainContent.IsEnabled = false;

            var editor = new SatEditor();
            var res = editor.ShowDialog();
            if (res == true)
            {
                Satellite satellite = editor.Satellite;
                try
                {
                    api.AddSatellite(satellite);
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка при обновлении данных: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MainContent.IsEnabled = true;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            MainContent.IsEnabled = false;

            if (SatellitesList.SelectedItem is null)
            {
                MessageBox.Show($"Не выбран спутник", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Satellite satellite = SatellitesList.SelectedItem as Satellite;
            try
            {
                api.DeleteSatellite(satellite.Name);
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка при обновлении данных: {ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MainContent.IsEnabled = true;
        }
    }
}
