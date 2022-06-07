using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.TLEManager
{
    internal class SateliteManagerModel
    {
        /// <summary>
        /// Доступные TLE
        /// </summary>
        public ReadOnlyObservableCollection<SateliteWrapper> AvailableTLEs { get; set; }
        private ObservableCollection<SateliteWrapper> _tles = new();

        public SateliteManagerModel()
        {
            AvailableTLEs = new(_tles);
        }

        public void GetCelestrakTLEs()
        {
            var url = new Uri("https://celestrak.com/NORAD/elements/resource.txt");

            var provider = new RemoteTleProvider(true, url);

            var newTles = provider.GetTles();

            if (newTles is null || newTles.Count == 0)
            {
                throw new NullReferenceException("Couldn't load TLEs");
            }

            foreach (var tle in newTles.Values)
            {
                _tles.Add(new SateliteWrapper
                {
                    IsChecked = false,
                    TLE = tle
                });
            }
        }
    }
}
