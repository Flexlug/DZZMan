using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZZMan.Models.SatelliteManager;
using DZZMan.Services;

namespace DZZMan.Models.MainWindow
{
    public class MainWindowModel
    {
        private ISatelliteProvider _satelliteProvider;
        
        public MainWindowModel(ISatelliteProvider satelliteProvider)
        {
            _satelliteProvider = satelliteProvider;
        }

        public List<SatelliteViewModel> GetAvaliableSatellites()
        {
            var satellites = _satelliteProvider.GetSatellites();
            var satelliteVMs = new List<SatelliteViewModel>();
            foreach (var satellite in satellites)
            {
                var vm = new SatelliteViewModel(satellite);
                satelliteVMs.Add(vm);
            }

            return satelliteVMs;
        }
    }
}
