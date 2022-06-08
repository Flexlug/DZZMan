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
        private Dictionary<int, Tle> _celestrakTles = new();
        private Dictionary<int, Tle> _dbTles = new();
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
            if (_celestrakTles.Count == 0)
                _celestrakTles.AddRange(_tleProvider.GetTles());

            if (!onlyFromDb)
                return _celestrakTles;

            if (_dbTles.Count != 0)
                return _dbTles;

            var satellites = _api.GetSatellites();
            foreach (var satellite in satellites)
            {
                if (_celestrakTles.ContainsKey(satellite.SCN))
                    _dbTles.Add(satellite.SCN, _celestrakTles[satellite.SCN]);
            }

            return _dbTles;
        }
    }
}
