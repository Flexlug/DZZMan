using System;
using System.Reactive;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGPdotNET.TLE;

using DZZMan.Models.TLEManager;
using ReactiveUI;
using Avalonia.Controls;
using DZZMan.Services;

namespace DZZMan.ViewModels
{
    public class SatelliteManagerViewModel : ViewModelBase
    {
        private SatelliteManagerModel _model;
        
        public ReadOnlyObservableCollection<SateliteWrapper> Satelites { get; }
        private ObservableCollection<SateliteWrapper> _satellites;

        public SatelliteManagerViewModel()
        {
            _model = ServiceProvider.Get<SatelliteManagerModel>();
            _satellites = new();

            OkButton = ReactiveCommand.Create<Window>((x) =>
            {
                x.Close();
            });

            try
            {
                _model.GetTLEs(false);
            }
            catch (NullReferenceException)
            {
                var errorMessageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка",
                    "Не удалось загузить TLE",
                    MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error);

                errorMessageBox.Show();
            }
        }

        public IReadOnlyCollection<SateliteWrapper> Ok()
        {
            return Satelites;
        }

        public ReactiveCommand<Window, Unit> OkButton { get; set; }
    }
}
