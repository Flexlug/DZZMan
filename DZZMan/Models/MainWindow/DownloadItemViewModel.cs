using System.ComponentModel;
using System.Runtime.CompilerServices;
using Humanizer.Bytes;
using JetBrains.Annotations;

namespace DZZMan.Models.MainWindow;

public class DownloadItemViewModel : INotifyPropertyChanged
{
    public string Name => _item.Name;
    
    public double Percentage
    {
        get => _percentage;
        set
        {
            _percentage = value;
            OnPropertyChanged(nameof(Percentage));
        }
    }
    private double _percentage;

    public ByteSize Speed => ByteSize.FromBytes(SpeedByte);
    
    public double SpeedByte
    {
        get => _speedByte;
        set
        {
            _speedByte = value;
            OnPropertyChanged(nameof(SpeedByte));
            OnPropertyChanged(nameof(Speed));
        }
    }
    private double _speedByte;
    
    public string Status 
    { 
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(Status);
        }
    }
    private string _status;
    
    private DownloadItem _item;
    
    public DownloadItemViewModel(DownloadItem item)
    {
        _item = item;
        Status = "Загрузка...";

        _item.Client.CurrentDownloadService.DownloadProgressChanged += (sender, args) =>
        {
            Percentage = args.ProgressPercentage;
            SpeedByte = args.BytesPerSecondSpeed;
        };
        
        _item.Client.CurrentDownloadService.DownloadFileCompleted += ((sender, args) =>
        {
            if (args.Cancelled)
            {
                Status = "Отменено";
                return;
            }
            
            if (args.Error is not null)
            {
                Status = $"Ошибка: {args.Error.Message}";
                return;
            }

            Status = "Завершено";

            Percentage = 100;
            SpeedByte = 0;
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}