using Avalonia.Controls;
using DZZMan.Models.MainWindow;
using DZZMan.Views;
using Mapsui;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using DZZMan.Services;
using SatelliteViewModel = DZZMan.Models.MainWindow.SatelliteViewModel;

namespace DZZMan.ViewModels
{
    /// <summary>
    /// ViewModel главного окна
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Список подгруженных спутников
        /// </summary>
        public ObservableCollection<SatelliteViewModel> Satellites { get; }

        /// <summary>
        /// Выбранный в DataGrid спутник
        /// </summary>
        public SatelliteViewModel SelectedSatellite
        {
            get => _selectedSatellite;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSatellite, value);
                this.RaisePropertyChanged(nameof(SelectedSatelliteHasInfoInDb));
            }
        }
        private SatelliteViewModel _selectedSatellite = null;

        public bool SelectedSatelliteHasInfoInDb => SelectedSatellite?.HasInfoInDB ?? false;

        private Map _map;

        private MainWindowModel _model;

        public MainWindowViewModel(Map map)
        {
            _map = map;
            _model = ServiceProvider.Get<MainWindowModel>();

            Satellites = new();

            OpenSateliteManager = ReactiveCommand.Create<Window>(async (x) => await LoadSateliteManager(x));

            ChangeStartDate = ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.Start));
            ChangeCurrentDate = ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.Current));
            ChangeEndDate = ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.End));
        }

        /// <summary>
        /// Открыть окно для добавления/удаления доступных TLE
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenSateliteManager { get; }
        public ReactiveCommand<Window, Unit> ChangeStartDate { get; }
        public ReactiveCommand<Window, Unit> ChangeCurrentDate { get; }
        public ReactiveCommand<Window, Unit> ChangeEndDate { get; }

        private async Task ChangeDateAsync(Window mainWindow, SelectedDate date)
        {
            if (SelectedSatellite is null)
            {
                await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка",
                    "Не удалось загузить TLE",
                    MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error).ShowDialog(mainWindow);
                return;
            }

            switch (date)
            {
                case SelectedDate.Start:
                    var start = await LoadDateChangerAsync(mainWindow, SelectedSatellite.TraceStartPoint);
                    SelectedSatellite.TraceStartPoint = start;
                    break;
                case SelectedDate.Current:
                    var current = await LoadDateChangerAsync(mainWindow, SelectedSatellite.CurrentPoint);
                    SelectedSatellite.CurrentPoint = current;
                    break;
                case SelectedDate.End:
                    var end = await LoadDateChangerAsync(mainWindow, SelectedSatellite.TraceEndPoint);
                    SelectedSatellite.TraceEndPoint = end;
                    break;
                default:
                    throw new Exception("Unknown enum. Expected SelectedDate");
            }

            OnMapChanged.Invoke();
        }

        enum SelectedDate
        {
            Start,
            Current,
            End
        }
        
        private async Task<DateTime> LoadDateChangerAsync(Window mainWindow, DateTime changeDate)
        {
            var dateTimeChanger = new DateChanger(changeDate);
            await dateTimeChanger.ShowDialog(mainWindow);

            var isCancelled = dateTimeChanger.ViewModel.IsCanceled;

            if (!isCancelled)
            {
                var date = dateTimeChanger.ViewModel.Date;
                var time = dateTimeChanger.ViewModel.Time;

                return new DateTime(date.Ticks + time.Ticks, DateTimeKind.Local);
            }

            return changeDate;
        }

        private async Task LoadSateliteManager(Window mainWindow)
        {
            var tleManager = new TLEManager();
            await tleManager.ShowDialog(mainWindow);

            var satelliteViewModels = _model.GetAvaliableSatellites();

            if (satelliteViewModels is null || satelliteViewModels.Count == 0)
            {
                return;
            }

            SatelliteViewModel lastSatelliteLayer = null;
            foreach (var satelliteVM in satelliteViewModels)
            {
                var existingLayers = _map.Layers.FindLayer(satelliteVM.Name);
                if (existingLayers is null || existingLayers.Count() == 0)
                {
                    _map.Layers.Add(satelliteVM.Layer);
                    Satellites.Add(satelliteVM);

                    lastSatelliteLayer = satelliteVM;
                }
            }

            SelectedSatellite = lastSatelliteLayer;
        }

        public delegate void MapChanged();
        
        /// <summary>
        /// Срабатывает при обновлении слоев
        /// </summary>
        public event MapChanged OnMapChanged;
    }
}
