using System.Collections.Generic;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using NetTopologySuite.Geometries;

namespace DZZMan.Models.ImageSearcher;

public class SearchPointLayer : Layer
{
    public double Latitude
    {
        get => _latitude;
        set
        {
            _latitude = value;
            OnPropertyChanged(nameof(Latitude));
            RegeneratePointFeature();
            UpdateMemoryProvider();
        }
    }
    private double _latitude;
    
    public double Longitude
    {
        get => _longitude;
        set
        {
            _longitude = value;
            OnPropertyChanged(nameof(Longitude));
            RegeneratePointFeature();
            UpdateMemoryProvider();
        }
    }
    private double _longitude;
    
    public Coordinate PointCoordinate { get; private set; } 
    
    private IFeature _centerPointFeature = null;
    
    public SearchPointLayer(double latitude, double longitude)
    {
        _latitude = latitude;
        _longitude = longitude;
    }

    private void RegeneratePointFeature()
    {
        PointCoordinate = SphericalMercator.FromLonLat(_longitude, _latitude).ToCoordinate();
        _centerPointFeature = new PointFeature(PointCoordinate.X, PointCoordinate.Y);
    }
    
    /// <summary>
    /// Обновить features слоя. Следует вызывать после генерации 
    /// </summary>
    private void UpdateMemoryProvider()
    {
        ClearCache();
        var features = new List<IFeature>();
       
        if (_centerPointFeature is not null)
            features.Add(_centerPointFeature);

        DataSource = new MemoryProvider(features);
        DataHasChanged();
    }
}