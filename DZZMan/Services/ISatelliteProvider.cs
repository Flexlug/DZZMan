using System.Collections.Generic;
using DZZMan.Models;

namespace DZZMan.Services;

public interface ISatelliteProvider
{
    public List<SatelliteWrapper> GetSatellites();
    public SatelliteWrapper GetSatelliteBySCN(int scn);
    public void AddOrUpdateSatellite(SatelliteWrapper satellite);
}