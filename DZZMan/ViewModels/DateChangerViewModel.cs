using System;
using System.Reactive;
using Avalonia.Controls;
using Mapsui.Providers.Wfs.Utilities;
using ReactiveUI;

namespace DZZMan.ViewModels;

public class DateChangerViewModel : ViewModelBase
{
    public DateTimeOffset Date
    {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref _date, value);
    }
    private DateTimeOffset _date;
    
    public TimeSpan Time
    {
        get => _time;
        set => this.RaiseAndSetIfChanged(ref _time, value);
    }
    private TimeSpan _time;

    public bool IsCanceled { get; set; } = false;

    public ReactiveCommand<Window, Unit> Ok { get; }
    public ReactiveCommand<Window, Unit> Cancel { get; }

    public DateChangerViewModel(DateTime dateTime)
    {
        Date = dateTime.Date;
        Time = dateTime.TimeOfDay;

        Ok = ReactiveCommand.Create<Window>(x =>
        {
            IsCanceled = false;
            x.Close();
        });

        Cancel = ReactiveCommand.Create<Window>(x =>
        {
            IsCanceled = true;
            x.Close();
        });
    }
}