using System;
using System.Reactive;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SGPdotNET.TLE;

using DZZMan.Models.TLEManager;
using ReactiveUI;
using Avalonia.Controls;

namespace DZZMan.ViewModels
{
    public class SateliteManagerViewModel : ViewModelBase
    {
        private SateliteManagerModel _model;
        
        public ReadOnlyObservableCollection<SateliteWrapper> Satelites { get; }

        public SateliteManagerViewModel()
        {
            _model = new();
            Satelites = _model.AvailableTLEs;

            OkButton = ReactiveCommand.Create<Window>((x) =>
            {
                x.Close();
            });

            try
            {
                _model.GetCelestrakTLEs();
            }
            catch (NullReferenceException)
            {
                var errorMessageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "Ошибка",
                    "Не удалось загузить TLE",
                    MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error);

                errorMessageBox.Show();
            }
        }

        public IReadOnlyCollection<SateliteWrapper> Ok()
        {
            return Satelites;
        }

        public ReactiveCommand<Window, Unit> OkButton { get; set; }
    }
}
