using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NetTopologySuite.Geometries;

namespace DZZMan.Models.ImageSearcher;

public class SearchPointViewModel : INotifyPropertyChanged
{
    public string LatitudeInput
    {
        get => _latitudeInput;
        set
        {
            _latitudeInput = value;
            OnPropertyChanged(nameof(LatitudeInput));

            if (double.TryParse(_latitudeInput, out var lat))
            {
                Layer.Latitude = lat;
            }
        }
    }
    private string _latitudeInput;

    public double Latitude => Layer.Latitude;

    public string LongitudeInput
    {
        get => _longitudeInput;
        set
        {
            _longitudeInput = value;
            OnPropertyChanged(nameof(LongitudeInput));
            
            if (double.TryParse(_longitudeInput, out var lon))
            {
                Layer.Longitude = lon;
            }
        }
    }
    private string _longitudeInput;
    
    public double Longitude => Layer.Longitude;

    public Coordinate PointCoordinate => Layer.PointCoordinate;

    public SearchPointLayer Layer { get; set; }
    
    public SearchPointViewModel(double longitude, double latitude)
    {
        Layer = new SearchPointLayer(latitude, longitude);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}