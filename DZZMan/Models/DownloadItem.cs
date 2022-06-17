using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CopernicusAPI.Model;
using JetBrains.Annotations;

namespace DZZMan.Models;

public class DownloadItem : INotifyPropertyChanged
{
    public string Name { get; }
    
    public DownloadClient Client;

    private Product _product;
    
    public DownloadItem(string login, string password, Product product)
    {
        Name = product.FileName;

        _product = product;
        
        Client = new DownloadClient(login, password, 1, "./Temp");
    }
    
    public async Task DownloadAsync() => await Client.Download(_product);

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}