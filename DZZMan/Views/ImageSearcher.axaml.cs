using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Mapsui;
using Mapsui.Tiling;
using Mapsui.UI.Avalonia;
using DZZMan.ViewModels;

namespace DZZMan.Views;

public partial class ImageSearcher : ReactiveWindow<ImageSearcherViewModel>
{
    public ImageSearcher(int SCN, DateTime date)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        InitializeMap();

        ViewModel = new ImageSearcherViewModel(SCN, date, MapControl.Map);

        ViewModel.OnMapChanged += () =>
        {
            MapControl.Refresh();
        };

        ViewModel.OnCenterMap += (lat, lon) =>
        {
            MapControl.Navigator.CenterOn(lon, lat);
        };
    }
    
    public ImageSearcher()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        InitializeMap();
    }

    private void InitializeMap()
    {
        var map = new Map();
        map.RotationLock = false;
        map.PanLock = false;

        map.Layers.Add(OpenStreetMap.CreateTileLayer());

        MapControl.Map = map;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        MapControl = this.Find<MapControl>("MapControl");
    }
}