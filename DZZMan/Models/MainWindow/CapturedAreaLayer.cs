using System;
using Mapsui.Layers;
using ReactiveUI;

namespace DZZMan.Models.MainWindow;

public class CapturedAreaLayer : Layer
{
    public DateTime TraceStartPoint
    {
        get
        {
            return _traceStartPoint;
        }
        set
        {
            _traceStartPoint = value;
            
        }
    }
    private DateTime _traceStartPoint;
}