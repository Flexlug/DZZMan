using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DZZMan.API;
using DZZMan.Models;
using MessageBox.Avalonia.Enums;

namespace DZZMan.AdminClient
{
    public partial class MainWindow : Window
    {
        private DZZManApi api = null;
        
        private ObservableCollection<Satellite> Satellites { get; set; } = new();
        
        public MainWindow()
        {
            InitializeComponent();
            SatellitesList.Items = Satellites;
        }

        private void Login_OnClick(object? sender, RoutedEventArgs e)
        {
            api = new(IPTB.Text);
            try
            {
                api.Login(TokenInput.Text);
            }
            catch (AggregateException)
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Неверный токен",
                    "Внимание",
                    ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Warning);
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    $"Неожиданная ошибка: {ex}", 
                    "Ошибка", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Warning);
                return;
            }

            MainContent.IsEnabled = true;
            Update();
        }
        
        private void Update()
        {
            var satellites = api.GetSatellites();

            Satellites = new(satellites);
            SatellitesList.Items = Satellites;
        }

        private void AddButton_OnClick(object? sender, RoutedEventArgs e)
        {
            MainContent.IsEnabled = false;

            var editor = new SatEditor();
            var resultSubmited = editor.ShowDialog<bool>(this).Result;
            
            if (resultSubmited)
            {
                Satellite satellite = editor.Satellite;
                try
                {
                    api.AddSatellite(satellite);
                    Update();
                }
                catch (Exception ex)
                {                    
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        $"Возникла ошибка при обновлении данных: {ex}", 
                        "Ошибка", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error);
                }
            }

            MainContent.IsEnabled = true;
        }

        private void RemoveButton_OnClick(object? sender, RoutedEventArgs e)
        {            
            MainContent.IsEnabled = false;

            if (SatellitesList.SelectedItem is null)
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Не выбран спутник", 
                    "Ошибка", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Warning);
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
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    $"Возникла ошибка при обновлении данных: {ex}", 
                    "Ошибка", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Error);
            }

            MainContent.IsEnabled = true;
        }

        private void InputElement_OnDoubleTapped(object? sender, RoutedEventArgs e)
        {            
            MainContent.IsEnabled = false;
            Satellite satellite = (sender as ListBoxItem).DataContext as Satellite;

            var editor = new SatEditor(satellite);
            var resultSubmited = editor.ShowDialog<bool>(this).Result;
            
            if (resultSubmited)
            {
                satellite = editor.Satellite;
                try
                {
                    api.UpdateSatellite(satellite);
                    Update();
                }
                catch(Exception ex)
                {
                    MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                        $"Возникла ошибка при обновлении данных: {ex}", 
                        "Ошибка", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error);
                }
            }

            MainContent.IsEnabled = true;
        }
    }
}
