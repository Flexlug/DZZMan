using Avalonia.Controls;
using DZZMan.Views;
using ReactiveUI;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace DZZMan.ViewModels
{
    /// <summary>
    /// ViewModel главного окна
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            OpenTLEManager = ReactiveCommand.Create<Window>(LoadTLEManager);
        }

        /// <summary>
        /// Открыть окно для добавления/удаления доступных TLE
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenTLEManager { get; }

        private void LoadTLEManager(Window mainWindow)
        {
            var tleManager = new TLEManager();
            var tlesResult = tleManager.ShowDialog<List<Tle>>(mainWindow);

            if (tlesResult is null)
            {
                return;
            }


        }
    }
}
