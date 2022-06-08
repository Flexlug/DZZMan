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
using DynamicData;
using DZZMan.Services;

namespace DZZMan.ViewModels
{
    public class SatelliteManagerViewModel : ViewModelBase
    {
        private SatelliteManagerModel _model;
        
        public ReadOnlyObservableCollection<SateliteWrapper> Satellites { get; }
        private ObservableCollection<SateliteWrapper> _satellites;

        public bool OnlyDb
        {
            get
            {
                return _onlyDb;
            }
            set
            {
                _onlyDb = value;
                UpdateSatellitesList();
            }
        }
        private bool _onlyDb = false;
        
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
                UpdateSatellitesList();
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

        private void UpdateSatellitesList()
        {
            var tles = _model.GetTLEs(OnlyDb);
            _satellites.Clear();
            foreach (var tle in tles)
            {
                _satellites.Add(new()
                {
                    IsChecked = false,
                    TLE = tle.Value
                });
            }
        }

        public ReactiveCommand<Window, Unit> OkButton { get; set; }
    }
}
