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
using MessageBox.Avalonia.DTO;
using ReactiveUI;

namespace DZZMan.Models.SatelliteManager
{
    internal class SatelliteManagerModel
    {   
        private Dictionary<int, Tle> _celestrakTles = new();
        private Uri _celestrakUri = new Uri("https://celestrak.com/NORAD/elements/resource.txt");
        
        private RemoteTleProvider _tleProvider;
        private ISatelliteProvider _satelliteProvider;
        private DZZManApi _api;

        public SatelliteManagerModel(DZZManApi api, ISatelliteProvider satelliteProvider)
        {
            _tleProvider = new RemoteTleProvider(true, _celestrakUri);
            _satelliteProvider = satelliteProvider;
            _api = api;
        }

        public List<SatelliteWrapper> GetSatellites(bool dbOnly)
        {
            if (_celestrakTles.Count == 0)
                _celestrakTles.AddRange(_tleProvider.GetTles());

            var satellitesVMs = new List<SatelliteWrapper>();
            var satelliteInfos = _api.GetSatellites()
                .ToDictionary(x => x.SCN, x => x);
            
            if (!dbOnly)
            {
                foreach (var tleKvp in _celestrakTles)
                {
                    if (satelliteInfos.TryGetValue((int)tleKvp.Value.NoradNumber, out var satellite))
                    {
                        satellitesVMs.Add(new(tleKvp.Value, satellite));
                    }
                    else
                    {
                        satellitesVMs.Add(new(tleKvp.Value));
                    }
                }
            }
            else
            {
                foreach (var satelliteKvp in satelliteInfos)
                {
                    if (_celestrakTles.TryGetValue(satelliteKvp.Key, out var tle))
                    {
                        satellitesVMs.Add(new(tle, satelliteKvp.Value));
                    }
                }
            }

            return satellitesVMs;
        }

        public void RegisterSatellites(List<SatelliteWrapper> satellites)
        {
            foreach (var satellite in satellites)
            {
                _satelliteProvider.AddOrUpdateSatellite(satellite);
            }
        }
    }
}
