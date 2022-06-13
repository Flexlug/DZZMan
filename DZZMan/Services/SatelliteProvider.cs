using System.Collections.Generic;
using System.Linq;
using DZZMan.Models;

namespace DZZMan.Services;

public class SatelliteProvider : ISatelliteProvider
{
    private Dictionary<int, SatelliteWrapper> _satellites = new();
    
    public SatelliteProvider() { }

    public void AddOrUpdateSatellite(SatelliteWrapper satellite)
    {
        if (!_satellites.TryAdd(satellite.SCN, satellite))
            _satellites[satellite.SCN] = satellite;
    }

    public List<SatelliteWrapper> GetSatellites() 
        => _satellites.Select(x => x.Value).ToList();

    public SatelliteWrapper GetSatelliteBySCN(int scn) 
        => _satellites[scn];
}