using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DZZMan.ViewModels;
using JetBrains.Annotations;
using MessageBox.Avalonia.Exceptions;
using ReactiveUI;

namespace DZZMan.Models.MainWindow;

public class SatelliteViewModel : INotifyPropertyChanged
{
    public DateTime TraceStartPoint
    {
        get => Layer.TraceStartPoint;
        set
        {
            Layer.TraceStartPoint = value;
            OnPropertyChanged(nameof(TraceStartPoint));
        }
    }

    public DateTime TraceEndPoint
    {
        get => Layer.TraceEndPoint;
        set
        {
            Layer.TraceEndPoint = value;
            OnPropertyChanged(nameof(TraceEndPoint));
        }
    }

    public DateTime CurrentPoint
    {
        get => Layer.CurrentPoint;
        set
        {
            Layer.CurrentPoint = value;
            OnPropertyChanged(nameof(CurrentPoint));
        }
    }

    public bool Enabled
    {
        get => Layer.Enabled;
        set
        {
            Layer.Enabled = value;
            OnPropertyChanged(nameof(Enabled));
        }
    }
    
    public string Name { get; }
    public int SCN { get; set; }
    public bool HasInfoInDB { get; }
    public Dictionary<string, string> OrbitInfo { get; }
    public SatelliteLayer Layer { get; }

    public SatelliteViewModel(SatelliteWrapper satellite)
    {
        Name = satellite.Tle.Name;
        HasInfoInDB = satellite.HasSatelliteInfo;
        OrbitInfo = satellite.GetOrbitParameters();
        SCN = satellite.SCN;
        Layer = new SatelliteLayer(satellite);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}