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
        private ICapturedAreaTasksProvider _capturedAreaTasksProvider;
        private IDownloadManager _downloadManager;
        
        public MainWindowModel(ISatelliteProvider satelliteProvider, ICapturedAreaTasksProvider capturedAreaTasksProvider, IDownloadManager downloadManager)
        {
            _satelliteProvider = satelliteProvider;
            _capturedAreaTasksProvider = capturedAreaTasksProvider;
            _downloadManager = downloadManager;
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

        public CapturedAreaViewModel GetCapturedAreaViewModel(int satelliteSCN)
        {
            var task = _capturedAreaTasksProvider.GetTask();
            var satellite = _satelliteProvider.GetSatelliteBySCN(satelliteSCN);
            
            if (task is null)
            {
                return null;
            }
            
            return new CapturedAreaViewModel(task, satellite);
        }

        public List<DownloadItemViewModel> GetDownloads()
        {
            var downloadItems = _downloadManager.GetDownloadQueue();
            var downloadItemVms = downloadItems.Select(x => new DownloadItemViewModel(x)).ToList();

            return downloadItemVms;
        }
    }
}
