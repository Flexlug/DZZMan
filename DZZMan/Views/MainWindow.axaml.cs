using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DZZMan.ViewModels;
using Mapsui.Tiling;
using ReactiveUI;

namespace DZZMan.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {
            MapControl.Map!.Layers.Add(OpenStreetMap.CreateTileLayer());
            MapControl.Map.RotationLock = false;
            MapControl.UnSnapRotationDegrees = 30;
            MapControl.ReSnapRotationDegrees = 5;
        }
    }
}
