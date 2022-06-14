using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DZZMan.Models.CapturedAreaCalc;
using JetBrains.Annotations;

namespace DZZMan.Models.MainWindow;

public class CapturedAreaViewModel : INotifyPropertyChanged
{
    public string Name
    {
        get => Layer.Name;
        set
        {
            Layer.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public DateTime AreaStartPoint
    {
        get => Layer.AreaEndPoint;
        set
        {
            Layer.AreaEndPoint = value;
            OnPropertyChanged(nameof(AreaStartPoint));
        }
    }

    public DateTime AreaEndPoint
    {
        get => Layer.AreaEndPoint;
        set
        {
            Layer.AreaEndPoint = value;
            OnPropertyChanged(nameof(AreaEndPoint));
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
    
    public CapturedAreaLayer Layer { get; }

    private SatelliteWrapper _satellite;
    
    public CapturedAreaViewModel(CapturedAreaTask task, SatelliteWrapper satellite)
    {
        Layer = new CapturedAreaLayer(task, satellite);

        AreaStartPoint = task.StartDate;
        AreaEndPoint = task.EndDate;
        Enabled = true;

        _satellite = satellite;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}