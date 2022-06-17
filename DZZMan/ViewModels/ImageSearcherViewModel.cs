using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaEdit.Utils;
using DynamicData;
using DZZMan.Models.ImageSearcher;
using DZZMan.Services;
using DZZMan.Views;
using Mapsui;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace DZZMan.ViewModels;

public class ImageSearcherViewModel : ViewModelBase
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

    public string LatitudeInput
    {
        get => _latitudeInput;
        set => this.RaiseAndSetIfChanged(ref _latitudeInput, value);
    }
    private string _latitudeInput;

    public string LongitudeInput
    {
        get => _longitudeInput;        
        set => this.RaiseAndSetIfChanged(ref _longitudeInput, value);
    }
    private string _longitudeInput;
    
    public bool ShowLoginPanel
    {
        get => _showLoginPanel;
        set => this.RaiseAndSetIfChanged(ref _showLoginPanel, value);
    }
    private bool _showLoginPanel = true;

    public bool MainPanelAccessible
    {
        get => _mainPanelAccessible;
        set => this.RaiseAndSetIfChanged(ref _mainPanelAccessible, value);
    }
    private bool _mainPanelAccessible = false;
    
    public bool IsCanceled { get; set; }

    public ObservableCollection<ProductViewModel> ProductList { get; set; } = new();

    public SearchPointViewModel LayerViewModel { get; set; }
    
    private ImageSearcherModel _model;
    private Map _map;

    public ImageSearcherViewModel(int SCN, DateTime currentDate, Map map)
    {
        _model = ServiceProvider.Get<ImageSearcherModel>();
        _map = map;
        
        var searchPointVM = _model.LoadSatellite(SCN, currentDate);
        LayerViewModel = searchPointVM;
        
        _map.Layers.Add(LayerViewModel.Layer);
        
        var now = DateTime.Now;

        StartDate = now - TimeSpan.FromDays(1);;
        StartTime = now.TimeOfDay;

        EndDate = now;
        EndTime = now.TimeOfDay;

        Login = ReactiveCommand.Create<Window>(async (x) => await TryLoginAsync(x));
        SetPoint = ReactiveCommand.Create<Unit>(async (x) => await SetPointAsync());
        StartSearch = ReactiveCommand.Create<Unit>(async (x) => await StartSearchAsync());
        
        Cancel = ReactiveCommand.Create<Window>((x) =>
        {
            IsCanceled = true;
            x.Close();
        });
        
        Ok = ReactiveCommand.Create<Window>(async (x) =>
        {
            var selectedProducts = ProductList
                .Where(x => x.Enabled)
                .Select(x => x.Product)
                .ToList();

            await _model.StartDownload(selectedProducts);
            
            IsCanceled = false;
            x.Close();
        });
    }
    
    public ReactiveCommand<Window, Unit> Login { get; }
    
    public ReactiveCommand<Unit, Unit> SetPoint { get; }
    
    public ReactiveCommand<Unit, Unit> StartSearch { get; }

    public ReactiveCommand<Window, Unit> Cancel { get; }
    
    public ReactiveCommand<Window, Unit> Ok { get; }

    private async Task StartSearchAsync()
    {
        var startDateTimeSpan = _startDate.Date.Ticks;
        var startTimeSpan = _startTime.Ticks;

        var endDateTimeSpan = _endDate.Date.Ticks;
        var endTimeSpan = _endTime.Ticks;

        var startDate = new DateTime(startDateTimeSpan + startTimeSpan, DateTimeKind.Local);
        var endDate = new DateTime(endDateTimeSpan + endTimeSpan, DateTimeKind.Local);

        var products = await _model.SearchAsync(LayerViewModel.Latitude, LayerViewModel.Longitude, startDate, endDate);

        foreach (var productVm in ProductList)
        {
            _map.Layers.Remove(productVm.Layer);
        }
        ProductList.Clear();
        
        foreach (var product in products)
        {
            var productVm = new ProductViewModel(product);
            ProductList.Add(productVm);
            _map.Layers.Add(productVm.Layer);
        }
        
        OnMapChanged.Invoke();
    }

    private async Task SetPointAsync()
    {
        LayerViewModel.LatitudeInput = _latitudeInput;
        LayerViewModel.LongitudeInput = _longitudeInput;
        
        if (LayerViewModel.PointCoordinate is not null)
        {
            OnMapChanged.Invoke();
            OnCenterMap.Invoke(LayerViewModel.PointCoordinate.X, LayerViewModel.PointCoordinate.Y);
        }
    }
    
    private async Task TryLoginAsync(Window window)
    {
        LoginPasswordAuthFormViewModel authVM;
        bool loginSuccess;
        
        var previousAttemptFailed = false;
        
        do
        {
            var authForm = new LoginPasswordAuthForm(previousAttemptFailed);
            await authForm.ShowDialog(window);    

            authVM = authForm.ViewModel;

            if (authVM is null ||
                authVM.IsCanceled ||
                string.IsNullOrEmpty(authVM.Login) || 
                string.IsNullOrEmpty(authVM.Password))
                return;
            
            loginSuccess = await _model.TryLoginAsync(authVM.Login, authVM.Password);
            previousAttemptFailed = true;
            
        } while (!loginSuccess);

        ShowLoginPanel = false;
        MainPanelAccessible = true;
    }

    public delegate void MapChanged();
    /// <summary>
    /// Срабатывает при обновлении слоев
    /// </summary>
    public event MapChanged OnMapChanged;

    
    public delegate void CenterMap(double x, double y);
    /// <summary>
    /// Срабатывает при запросах на центрирование карты
    /// </summary>
    public event CenterMap OnCenterMap;
}