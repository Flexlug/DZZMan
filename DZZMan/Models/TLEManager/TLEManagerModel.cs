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
        private ObservableCollection<TLEWrapper> tles = new();
        public ReadOnlyObservableCollection<TLEWrapper> AvailableTLEs { get; set; }

        public TLEManagerModel()
        {
            AvailableTLEs = new(tles);
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
                tles.Add(new TLEWrapper
                {
                    IsChecked = false,
                    TLE = tle
                });
            }
        }
    }
}
