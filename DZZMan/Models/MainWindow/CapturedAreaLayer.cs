﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using DZZMan.Models.CapturedAreaCalc;
using DZZMan.Utils;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts.Extensions;
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
    
    private SatelliteWrapper _satellite;
    private CapturedAreaTask _task;

    public CapturedAreaLayer(CapturedAreaTask task, SatelliteWrapper satellite)
    {
        _areaStartPoint = task.StartDate;
        _areaEndPoint = task.EndDate;
        _satellite = satellite;
        _task = task;
        
        Name = $"{satellite.Name}={task.Name}";
        
        CountCapturedAreaFootprint();
    }

    private void CountCapturedAreaFootprint()
    {
        switch (_satellite.Satellite.Sensor.SensorType)
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
        var result = SatelliteMath.CalculateCapturedAreaForScanner(
            _satellite.Tle, 
            _satellite.Satellite, 
            _task.StartDate, 
            _task.EndDate, 
            _task.SkipDark);

        _footprintFeature = result
            .Select(x => x.ToFeature())
            .ToList();
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