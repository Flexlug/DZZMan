using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;
using Mapsui;
using Mapsui.Tiling;
using ReactiveUI;

namespace DZZMan.Views
{
    public partial class MainWindow : ReactiveWindow<SateliteManagerViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();

            DataContext = new MainWindowViewModel(MapControl.Map);
        }

        private void InitializeMap()
        {
            var map = new Map();
            map.RotationLock = false;
            map.PanLock = false;

            var osm = OpenStreetMap.CreateTileLayer();
            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            MapControl.Map = map;
        }
    }
}
