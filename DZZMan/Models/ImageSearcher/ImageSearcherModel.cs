using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CopernicusAPI;
using CopernicusAPI.Model;
using DZZMan.Models.ImageSources;
using DZZMan.Services;
using DZZMan.Utils;
using DZZMan.ViewModels;

namespace DZZMan.Models.ImageSearcher;

public class ImageSearcherModel
{   
    private CopernicusApi _api;
    private ISatelliteProvider _satelliteProvider;
    private IDownloadManager _downloadManager;
    private SatelliteWrapper _satellite;

    private CopernicusSource _source;
    
    public ImageSearcherModel(CopernicusApi api, ISatelliteProvider satelliteProvider, IDownloadManager downloadManager)
    {
        _api = api;
        _satelliteProvider = satelliteProvider;
        _downloadManager = downloadManager;
    }

    public SearchPointViewModel LoadSatellite(int SCN, DateTime date)
    {
        var satellite = _satelliteProvider.GetSatelliteBySCN(SCN);
        _satellite = satellite;

        _source = _satellite.Satellite.ImageSources.Where(x => x is CopernicusSource).First() as CopernicusSource;
        
        var pos = SatelliteMath.GetSatellitePosition(_satellite.Tle, date);
        var searchPointViewModel = new SearchPointViewModel(pos.Latitude.Degrees, pos.Longitude.Degrees);

        return searchPointViewModel;
    }

    public async Task<bool> TryLoginAsync(string login, string password)
    {
        var authRes = await _api.LoginAsync(login, password);
        
        if (authRes)
        {
            _downloadManager.Auth(login, password);
        }

        return authRes;
    }

    public async Task<List<Product>> SearchAsync(double lat, double lon, DateTime start, DateTime end)
    {
        var name = _source.DownloadParametes["name"];
        var point = new[] { lon, lat };

        var result = await _api.GetProductsAsync(name, point, start, end);
        return result.Products;
    }

    public async Task StartDownload(List<Product> products)
    {
        foreach (var product in products)
        {
            await _downloadManager.AddDownload(product);
        }
    }
}