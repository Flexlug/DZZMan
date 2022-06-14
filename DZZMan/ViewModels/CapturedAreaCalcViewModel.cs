using System;
using System.Reactive;
using System.Text;
using Avalonia.Controls;
using BruTile.Wmts.Generated;
using DZZMan.Models.CapturedAreaCalc;
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
    
    public bool SkipDark { get; set; }

    public bool IsCanceled { get; set; } = false;

    private CapturedAreaCalcModel _model;
    
    public CapturedAreaCalcViewModel()
    {
        _model = Services.ServiceProvider.Get<CapturedAreaCalcModel>();
        
        var now = DateTime.Now;

        StartDate = now;
        StartTime = now.TimeOfDay;
        
        EndDate = now + TimeSpan.FromDays(1);
        EndTime = now.TimeOfDay;

        Ok = ReactiveCommand.Create<Window>(x =>
        {
            var startDateTimeSpan = _startDate.Ticks;
            var startTimeSpan = _startTime.Ticks;

            var endDateTimeSpan = _endDate.Ticks;
            var endTimeSpan = _endTime.Ticks;

            var startDate = new DateTime(startDateTimeSpan + startTimeSpan, DateTimeKind.Local);
            var endDate = new DateTime(endDateTimeSpan + endTimeSpan, DateTimeKind.Local);

            var nameBuilder = new StringBuilder();
            nameBuilder.Append(startDate.ToShortDateString());
            nameBuilder.Append('-');
            nameBuilder.Append(startDate.ToShortTimeString());
            nameBuilder.Append('=');
            nameBuilder.Append(endDate.ToShortDateString());
            nameBuilder.Append('-');
            nameBuilder.Append(endDate.ToShortTimeString());
            nameBuilder.Append('-');
            nameBuilder.Append(SkipDark ? "skip-dark" : "show-all");
            
            
            _model.RegisterTask(new()
            {
                Name = nameBuilder.ToString(),
                StartDate = startDate,
                EndDate = endDate,
                SkipDark = SkipDark
            });
            
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