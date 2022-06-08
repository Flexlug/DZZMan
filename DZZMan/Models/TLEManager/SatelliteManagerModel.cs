using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaEdit.Utils;
using DZZMan.API;
using DZZMan.Services;

namespace DZZMan.Models.TLEManager
{
    internal class SatelliteManagerModel
    {   
        private Dictionary<int, Tle> _tles = new();
        private Uri _celestrakUri = new Uri("https://celestrak.com/NORAD/elements/resource.txt");
        
        private RemoteTleProvider _tleProvider;
        private DZZManApi _api;


        public SatelliteManagerModel(DZZManApi api)
        {
            _tleProvider = new RemoteTleProvider(true, _celestrakUri);
            _api = api;
        }

        public Dictionary<int, Tle> GetTLEs(bool onlyFromDb)
        {
            if (_tles.Count == 0)
                _tles.AddRange(_tleProvider.GetTles());

            if (onlyFromDb)
            {
                var satellites = _api.GetSatellites();
                foreach (var satellite in satellites)
                {
                    
                }
            }
            
            return _tles;
        }
    }
}
