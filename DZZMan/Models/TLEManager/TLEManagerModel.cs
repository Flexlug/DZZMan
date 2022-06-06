using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZZMan.Models.TLEManager
{
    internal class TLEManagerModel
    {
        /// <summary>
        /// Доступные TLE
        /// </summary>
        public ReadOnlyObservableCollection<TLEWrapper> AvailableTLEs { get; set; }
        private ObservableCollection<TLEWrapper> _tles = new();

        public TLEManagerModel()
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
                _tles.Add(new TLEWrapper
                {
                    IsChecked = false,
                    TLE = tle
                });
            }
        }
    }
}
