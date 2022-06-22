using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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
        public MapControl mapView;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();

            var viewModel = new MainWindowViewModel(mapView.Map);
            DataContext = viewModel;

            viewModel.OnMapChanged += () =>
            {
                mapView.Refresh();
            };
        }

        private void InitializeMap()
        {
            mapView = this.Find<MapControl>("MapControl");
            
            var map = new Map();
            map.RotationLock = false;
            map.PanLock = false;

            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            mapView.Map = map;
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
