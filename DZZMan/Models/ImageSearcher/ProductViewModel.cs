using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CopernicusAPI.Model;
using Humanizer.Bytes;
using JetBrains.Annotations;
using Mapsui.UI.Avalonia.Extensions;

namespace DZZMan.Models.ImageSearcher;

public class ProductViewModel: INotifyPropertyChanged
{
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
    public double CloudCover { get; }
    public DateTime BeginPosition { get; }
    public DateTime EndPosition { get; }
    public string Size { get; }
    public FootprintLayer Layer { get; }

    public Product Product { get; }
    
    public ProductViewModel(Product product)
    {
        Product = product;

        Name = product.Title;
        CloudCover = product.CloudCoverPercentage;
        BeginPosition = product.BeginPosition;
        EndPosition = product.EndPosition;
        Layer = new FootprintLayer(product);
        Size = product.Size;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}