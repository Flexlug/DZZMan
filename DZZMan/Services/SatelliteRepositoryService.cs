using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AvaloniaEdit.Utils;
using DZZMan.API;
using DZZMan.Models.TLEManager;
using SGPdotNET.TLE;

namespace DZZMan.Services;

public class SatelliteRepositoryService : ISatelliteRepositoryService
{
    private Dictionary<int, Tle> _tles = new();
    private Uri _celestrakUri = new Uri("https://celestrak.com/NORAD/elements/resource.txt");
    private RemoteTleProvider _tleProvider;
    private DZZManApi _api;
    
    public SatelliteRepositoryService(DZZManApi api)
    {
        _tleProvider = new(true, _celestrakUri);
        _api = api;
    }
    
    public List<Tle> GetTLEs(bool onlyFromDb)
    {
        if (_tles.Count == 0)
            _tles.AddRange(_tleProvider.GetTles());

        return _tles.Select(x => x.Value).ToList();
    }
}