using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;
using Mapsui;
using Mapsui.Tiling;
using Mapsui.UI.Avalonia;
using ReactiveUI;

namespace DZZMan.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();

            var viewModel = new MainWindowViewModel(MapControl.Map);
            DataContext = viewModel;

            viewModel.OnMapChanged += () =>
            {
                MapControl.Refresh();
            };
        }

        private void InitializeMap()
        {
            var map = new Map();
            map.RotationLock = false;
            map.PanLock = false;

            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            MapControl.Map = map;
        }
    }
}
