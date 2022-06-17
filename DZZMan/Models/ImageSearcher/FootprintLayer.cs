using System;
using System.Collections.Generic;
using System.Linq;
using CopernicusAPI.Model;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Providers;
using NetTopologySuite.Geometries;

namespace DZZMan.Models.ImageSearcher;

public class FootprintLayer : Layer
{
    private IFeature _footprintFeature = null;

    private string _footprintString;
    
    public FootprintLayer(Product product)
    {
        _footprintString = product.Footprint;
        
        GeneratePolygonFeature();
        UpdateMemoryProvider();
    }

    private void GeneratePolygonFeature()
    {
        string[] raw_cords = _footprintString
            .Replace("MULTIPOLYGON", string.Empty)
            .Replace(")", string.Empty)
            .Replace("(", string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        List<List<double>> polygonPoints = new();
        for (int i = 0; i < raw_cords.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(raw_cords[i]))
                continue;

            var point = raw_cords[i]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => double.Parse(x.Trim().Replace('.', ',')))
                .ToList();


            polygonPoints.Add(point);
        }

        List<Coordinate> polygonCoordinates = new();
        foreach (var point in polygonPoints)
        {
            var x = point[0];
            var y = point[1];

            polygonCoordinates.Add(SphericalMercator.FromLonLat(y, x).ToCoordinate());
        }
        
        var geometryFactory = new GeometryFactory();
        var pointArray = polygonCoordinates.ToArray();
        var polygon = geometryFactory.CreatePolygon(pointArray);

        _footprintFeature = polygon.ToFeature();
    }

    /// <summary>
    /// Обновить features слоя. Следует вызывать после генерации 
    /// </summary>
    private void UpdateMemoryProvider()
    {
        ClearCache();
        var features = new List<IFeature>();
       
        if (_footprintFeature is not null)
            features.Add(_footprintFeature);

        DataSource = new MemoryProvider(features);
        DataHasChanged();
    }
}