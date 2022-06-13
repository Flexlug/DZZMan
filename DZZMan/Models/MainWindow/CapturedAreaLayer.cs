using System;
using System.Collections.Generic;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Providers;
using ReactiveUI;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace DZZMan.Models.MainWindow;

public class CapturedAreaLayer : Layer
{
    public DateTime AreaStartPoint
    {
        get
        {
            return _areaStartPoint;
        }
        set
        {
            _areaStartPoint = value;            
        }
    }
    private DateTime _areaStartPoint;
    
    public DateTime AreaEndPoint
    {
        get
        {
            return _areaEndPoint;
        }
        set
        {
            _areaEndPoint = value;            
        }
    }
    private DateTime _areaEndPoint;

    private IEnumerable<IFeature> _footprintFeature = null;
    
    private Tle _tle;
    private Sgp4 _sgp;
    private List<GeodeticCoordinate> _traceGeodeticCoordinates;
    private List<Coordinate> _traceCoordinates;
    private Satellite _satellite;

    public CapturedAreaLayer(SatelliteLayer layer, Satellite satellite, DateTime areaStart, DateTime areaEnd)
    {
        _areaStartPoint = areaStart;
        _areaEndPoint = areaEnd;

        CountCapturedAreaFootprint();
    }

    private void CountCapturedAreaFootprint()
    {
        switch (_satellite.Sensor.SensorType)
        {
            case SensorType.Scanner:
                CountCaptureadAreaForScanner();
                break;
            case SensorType.Frame:
                CountCaptureadAreaForFrame();
                break;
            default:
                throw new Exception("Unknown enum type");
        }
    }

    private void CountCaptureadAreaForFrame()
    {
        
    }

    private void CountCaptureadAreaForScanner()
    {
        
    }


    /// <summary>
    /// Обновить features слоя. Следует вызывать после генерации 
    /// </summary>
    private void UpdateMemoryProvider()
    {
        ClearCache();
        var features = new List<IFeature>();

        if (_footprintFeature is not null)
            features.AddRange(_footprintFeature);
        
        DataSource = new MemoryProvider(features);
        DataHasChanged();
    }
}