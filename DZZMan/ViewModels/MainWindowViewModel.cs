using Avalonia.Controls;
using DZZMan.Models.MainWindow;
using DZZMan.ViewModels.Models;
using DZZMan.Views;
using Mapsui;
using Mapsui.Layers;
using NetTopologySuite.Geometries;
using ReactiveUI;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using BruTile.Wmts.Generated;
using DZZMan.Models.TLEManager;

namespace DZZMan.ViewModels
{
    /// <summary>
    /// ViewModel главного окна
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Список подгруженных спутников
        /// </summary>
        public ObservableCollection<SateliteLayer> SateliteLayers { get; set; }

        /// <summary>
        /// Выбранный в DataGrid слой
        /// </summary>
        public SateliteLayer SelectedLayer
        {
            get => _selectedLayer;
            set => this.RaiseAndSetIfChanged(ref _selectedLayer, value);
        }
        private SateliteLayer _selectedLayer = null;
    

        private Map _map;

        private MainWindowModel _model;

        public MainWindowViewModel(Map map)
        {
            _map = map;
            _model = new();

            SateliteLayers = new();

            OpenSateliteManager = ReactiveCommand.Create<Window>(async (x) => await LoadSateliteManager(x));
        }

        /// <summary>
        /// Открыть окно для добавления/удаления доступных TLE
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenSateliteManager { get; }

        private async Task LoadSateliteManager(Window mainWindow)
        {
            var tleManager = new TLEManager();
            await tleManager.ShowDialog(mainWindow);

            var tlesResult = tleManager?.ViewModel?.Satelites;

            if (tlesResult is null)
            {
                return;
            }

            SateliteLayer lastSateliteLayer = null;
            foreach (var tleWrapper in tlesResult.Where(x => x.IsChecked))
            {
                var existingLayers = _map.Layers.FindLayer(tleWrapper.TLE.Name);
                if (existingLayers is null || existingLayers.Count() == 0)
                {
                    var sateliteLayer = _model.CreateSateliteLayer(tleWrapper.TLE);
                    _map.Layers.Add(sateliteLayer);
                    SateliteLayers.Add(sateliteLayer);

                    lastSateliteLayer = sateliteLayer;
                }
            }

            SelectedLayer = lastSateliteLayer;
        }
    }
}
