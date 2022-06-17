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
using Avalonia.Rendering;
using DynamicData;
using DZZMan.Services;
using Topten.RichTextKit.Editor;
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
        public ObservableCollection<SatelliteViewModel> Satellites { get; } = new();

        /// <summary>
        /// Список просчитанных площадей съемки
        /// </summary>
        public ObservableCollection<CapturedAreaViewModel> CapturedAreas { get; } = new();

        /// <summary>
        /// Список текущих загрузок
        /// </summary>
        public ObservableCollection<DownloadItemViewModel> Downloads { get; } = new();

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
                this.RaisePropertyChanged(nameof(SelectedSatelliteHasImageSources));
            }
        }
        private SatelliteViewModel _selectedSatellite = null;

        public bool SelectedSatelliteHasInfoInDb => SelectedSatellite?.HasInfoInDB ?? false;

        public bool SelectedSatelliteHasImageSources => SelectedSatellite?.HasImageSources ?? false;
        
        /// <summary>
        /// Выбранная отснятая область
        /// </summary>
        public CapturedAreaViewModel SelectedCapturedArea
        {
            get => _selectedCapturedArea;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCapturedArea, value);
                this.RaisePropertyChanged(nameof(SelectedCapturedArea));
            }
        }
        private CapturedAreaViewModel _selectedCapturedArea;

        public bool AnyCapturedAreaExists => CapturedAreas.Count > 0;

        private Map _map;

        private MainWindowModel _model;

        public MainWindowViewModel(Map map)
        {
            _map = map;
            _model = ServiceProvider.Get<MainWindowModel>();

            OpenSateliteManager = ReactiveCommand.Create<Window>(async (x) => await LoadSateliteManager(x));
            OpenCapturedAreaCalc = ReactiveCommand.Create<Window>(async (x) => await LoadCapturedAreaCalc(x));
            OpenImageSearcher = ReactiveCommand.Create<Window>(async (x) => await LoadImageSearcherAsync(x));

            ChangeStartDate = ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.Start));
            ChangeCurrentDate =
                ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.Current));
            ChangeEndDate = ReactiveCommand.Create<Window>(async (x) => await ChangeDateAsync(x, SelectedDate.End));
        }


        /// <summary>
        /// Открыть окно для добавления/удаления доступных TLE
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenSateliteManager { get; }

        /// <summary>
        /// Открыть окно для составления запроса на расчет 
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenCapturedAreaCalc { get; }
        
        /// <summary>
        /// Открыть окно для поиска космических снимков
        /// </summary>
        public ReactiveCommand<Window, Unit> OpenImageSearcher { get; }

        /// <summary>
        /// Изменить дату, с которой должен начинаться рассчет трассы
        /// </summary>
        public ReactiveCommand<Window, Unit> ChangeStartDate { get; }

        /// <summary>
        /// Изменить дату, на которую должна отображаться позиция спутника
        /// </summary>
        public ReactiveCommand<Window, Unit> ChangeCurrentDate { get; }

        /// <summary>
        /// Изменить дату, до которой должен идти расчет трассы
        /// </summary>
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

                return new DateTime(date.Date.Ticks + time.Ticks, DateTimeKind.Local);
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

        private async Task LoadCapturedAreaCalc(Window window)
        {
            var capturedAreaCalc = new CapturedAreaCalc();
            await capturedAreaCalc.ShowDialog(window);

            var capturedAreaTask = _model.GetCapturedAreaViewModel(SelectedSatellite.SCN);

            if (capturedAreaTask is null)
                return;

            var existingLayers = _map.Layers.FindLayer(capturedAreaTask.Name);
            if (existingLayers is null || existingLayers.Count() == 0)
            {
                _map.Layers.Add(capturedAreaTask.Layer);
                CapturedAreas.Add(capturedAreaTask);
            }
        }

        private async Task LoadImageSearcherAsync(Window window)
        {   
            var imageDownloader = new ImageSearcher(SelectedSatellite.SCN, SelectedSatellite.CurrentPoint);
            await imageDownloader.ShowDialog(window);

            if (imageDownloader.ViewModel.IsCanceled)
                return;

            var downloadItemVms = _model.GetDownloads();
            if (downloadItemVms is null || downloadItemVms.Count == 0)
                return;
            
            Downloads.AddRange(downloadItemVms);
        }

        public delegate void MapChanged();

        /// <summary>
        /// Срабатывает при обновлении слоев
        /// </summary>
        public event MapChanged OnMapChanged;
    }
}
