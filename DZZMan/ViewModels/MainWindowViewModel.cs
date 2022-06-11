using Avalonia.Controls;
using DZZMan.Models.MainWindow;
using DZZMan.Views;
using Mapsui;
using Mapsui.Layers;
using NetTopologySuite.Geometries;
using ReactiveUI;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using BruTile.Wmts.Generated;
using DZZMan.Models.TLEManager;

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
        public ObservableCollection<SateliteLayer> SateliteLayers { get; }

        /// <summary>
        /// Выбранный в DataGrid слой
        /// </summary>
        public SateliteLayer SelectedLayer
        {
            get => _selectedLayer;
            set => this.RaiseAndSetIfChanged(ref _selectedLayer, value);
        }

        private SateliteLayer _selectedLayer = null;

        private Map _map;

        private MainWindowModel _model;

        public MainWindowViewModel(Map map)
        {
            _map = map;
            _model = new();

            SateliteLayers = new();

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
            if (SelectedLayer is null)
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
                    var start = await LoadDateChangerAsync(mainWindow, SelectedLayer.TraceStartPoint);
                    SelectedLayer.TraceStartPoint = start;
                    break;
                case SelectedDate.Current:
                    var current = await LoadDateChangerAsync(mainWindow, SelectedLayer.CurrentPoint);
                    SelectedLayer.CurrentPoint = current;
                    break;
                case SelectedDate.End:
                    var end = await LoadDateChangerAsync(mainWindow, SelectedLayer.TraceEndPoint);
                    SelectedLayer.TraceEndPoint = end;
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

                return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes,
                    time.Seconds, DateTimeKind.Utc);
            }

            return changeDate;
        }

        private async Task LoadSateliteManager(Window mainWindow)
        {
            var tleManager = new TLEManager();
            await tleManager.ShowDialog(mainWindow);

            var tlesResult = tleManager?.ViewModel?.Satellites;

            if (tlesResult is null)
            {
                return;
            }

            SateliteLayer lastSateliteLayer = null;
            foreach (var tleWrapper in tlesResult.Where(x => x.IsChecked))
            {
                var existingLayers = _map.Layers.FindLayer(tleWrapper.TLE.Name);
                if (existingLayers is null || existingLayers.Count() == 0)
                {
                    var sateliteLayer = _model.CreateSateliteLayer(tleWrapper.TLE);
                    _map.Layers.Add(sateliteLayer);
                    SateliteLayers.Add(sateliteLayer);

                    lastSateliteLayer = sateliteLayer;
                }
            }

            SelectedLayer = lastSateliteLayer;
        }

        public delegate void MapChanged();
        
        /// <summary>
        /// Срабатывает при обновлении слоев
        /// </summary>
        public event MapChanged OnMapChanged;
    }
}
