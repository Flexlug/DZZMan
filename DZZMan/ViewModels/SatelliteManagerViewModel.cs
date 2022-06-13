using System;
using System.Reactive;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGPdotNET.TLE;

using DZZMan.Models.SatelliteManager;
using ReactiveUI;
using Avalonia.Controls;
using DynamicData;
using DZZMan.Services;

namespace DZZMan.ViewModels
{
    public class SatelliteManagerViewModel : ViewModelBase
    {
        private SatelliteManagerModel _model;
        
        public ReadOnlyObservableCollection<SatelliteViewModel> Satellites { get; }
        private ObservableCollection<SatelliteViewModel> _satellites = new();

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
            Satellites = new(_satellites);
                
            OkButton = ReactiveCommand.Create<Window>((x) =>
            {
                var selectedSatellites = _satellites
                    .Where(x => x.IsChecked)
                    .Select(x => x.SatelliteWrapper)
                    .ToList();
                
                _model.RegisterSatellites(selectedSatellites);
                
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
            var tles = _model.GetSatellites(OnlyDb);
            _satellites.Clear();
            foreach (var tle in tles)
            {
                _satellites.Add(new(tle));
            }
        }

        public ReactiveCommand<Window, Unit> OkButton { get; }
    }
}
