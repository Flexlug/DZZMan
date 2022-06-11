using System;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;

namespace DZZMan.ViewModels;

public class CapturedAreaCalcViewModel : ViewModelBase
{
    public DateTimeOffset StartDate
    {
        get => _startDate;
        set => this.RaiseAndSetIfChanged(ref _startDate, value);
    }
    private DateTimeOffset _startDate;
    
    public TimeSpan StartTime
    {
        get => _startTime;
        set => this.RaiseAndSetIfChanged(ref _startTime, value);
    }
    private TimeSpan _startTime;
    
    public DateTimeOffset EndDate
    {
        get => _endDate;
        set => this.RaiseAndSetIfChanged(ref _endDate, value);
    }
    private DateTimeOffset _endDate;
    
    public TimeSpan EndTime
    {
        get => _endTime;
        set => this.RaiseAndSetIfChanged(ref _endTime, value);
    }
    private TimeSpan _endTime;

    public bool IsCanceled { get; set; } = false;
    
    public CapturedAreaCalcViewModel()
    {
        var now = DateTime.Now;

        StartDate = now;
        StartTime = now.TimeOfDay;
        
        EndDate = now + TimeSpan.FromDays(1);
        EndTime = now.TimeOfDay;

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

    public ReactiveCommand<Window, Unit> Ok;
    public ReactiveCommand<Window, Unit> Cancel;
}