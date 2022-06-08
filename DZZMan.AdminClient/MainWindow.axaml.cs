using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DZZMan.API;
using DZZMan.Models;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

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
                    "Внимание",
                    "Неверный токен",
                    ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Warning).ShowDialog(this).ConfigureAwait(false);
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка", 
                    $"Неожиданная ошибка: {ex}", 
                    ButtonEnum.Ok, 
                    MessageBox.Avalonia.Enums.Icon.Warning).ShowDialog(this).ConfigureAwait(false);
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
            using (var source = new CancellationTokenSource())
            {
                editor.ShowDialog(this).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source.Token);
            }
            var resultSubmited = editor.Result;
            
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
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this).ConfigureAwait(false);
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
                    MessageBox.Avalonia.Enums.Icon.Warning).ShowDialog(this).ConfigureAwait(false);
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
                    MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this).ConfigureAwait(false);
            }

            MainContent.IsEnabled = true;
        }

        private void InputElement_OnDoubleTapped(object? sender, RoutedEventArgs e)
        {            
            MainContent.IsEnabled = false;
            var satellite = (sender as StackPanel).DataContext as Satellite;

            var editor = new SatEditor();
            using (var source = new CancellationTokenSource())
            {
                editor.ShowDialog(this).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                Dispatcher.UIThread.MainLoop(source.Token);
            }
            var resultSubmited = editor.Result;
            
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
                        "Ошибка", 
                        $"Возникла ошибка при обновлении данных: {ex}", 
                        ButtonEnum.Ok, 
                        MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(this).ConfigureAwait(false);
                }
            }

            MainContent.IsEnabled = true;
        }
    }
}
