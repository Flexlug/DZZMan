using DZZMan.Models.TLEManager;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.ViewModels
{
    public class TLEManagerViewModel : ViewModelBase
    {
        private TLEManagerModel model;
        
        public ReadOnlyObservableCollection<TLEWrapper> TLEs { get; }

        public TLEManagerViewModel()
        {
            model = new();
            TLEs = model.AvailableTLEs;

            try
            {
                model.GetCelestrakTLEs();
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
    }
}
