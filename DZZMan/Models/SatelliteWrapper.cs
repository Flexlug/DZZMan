
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGPdotNET.Propagation;

namespace DZZMan.Models
{
    /// <summary>
    /// Обертка поверх TLE для окна TLEManager
    /// </summary>
    public class SatelliteWrapper
    {
        /// <summary>
        /// Название спутника
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Номер спутника по каталогу NORAD
        /// </summary>
        public int SCN { get; set; }
        
        /// <summary>
        /// Полная версия TLE
        /// </summary>
        public Tle Tle { get; set; }
        
        /// <summary>
        /// Обозначает, есть ли информация о спутнике в БД
        /// </summary>
        public bool HasSatelliteInfo { get; set; }
        
        /// <summary>
        /// Обозначает, есть ли в БД информация об источниках скачивания
        /// </summary>
        public bool HasImageSources { get; set; }
        
        /// <summary>
        /// Информация о спутнике из БД
        /// </summary>
        public Satellite Satellite { get; set; }
        
        public TimeSpan Period { get; set; }

        private Sgp4 _sgp;

        public SatelliteWrapper(Tle tle)
        {
            Name = tle.Name;
            Tle = tle;
            SCN = (int)tle.NoradNumber;
            HasSatelliteInfo = false;
            HasImageSources = false;
            Satellite = null;
        }

        public SatelliteWrapper(Tle tle, Satellite satellite)
        {
            Name = tle.Name;
            Tle = tle;
            SCN = (int)tle.NoradNumber;
            HasSatelliteInfo = true;
            HasImageSources = satellite.ImageSources.Count != 0;
            Satellite = satellite;
        }

        public Dictionary<string, string> GetOrbitParameters()
        {
            _sgp = new Sgp4(Tle);

            Period = TimeSpan.FromMinutes(_sgp.Orbit.Period);
            
            var orbitParams = new Dictionary<string, string>();
            orbitParams.Add("Apogee, km", _sgp.Orbit.Apogee.ToString());
            orbitParams.Add("Perigee, km", _sgp.Orbit.Perigee.ToString());
            orbitParams.Add("Eccentricity", _sgp.Orbit.Eccentricity.ToString());
            orbitParams.Add("TLE epoch", _sgp.Orbit.Epoch.ToString());
            orbitParams.Add("Inclination, °", _sgp.Orbit.Inclination.Degrees.ToString());
            orbitParams.Add("Period", _sgp.Orbit.Period.ToString());
            orbitParams.Add("Argument Perigee, °", _sgp.Orbit.ArgumentPerigee.Degrees.ToString());
            orbitParams.Add("Ascending Node, °", _sgp.Orbit.AscendingNode.Degrees.ToString());
            orbitParams.Add("BSTAR drag term", _sgp.Orbit.BStar.ToString());
            orbitParams.Add("Mean Anomoly, °", _sgp.Orbit.MeanAnomoly.Degrees.ToString());
            orbitParams.Add("Mean Motion", _sgp.Orbit.MeanMotion.ToString());
            orbitParams.Add("Recovered Mean Motion", _sgp.Orbit.RecoveredMeanMotion.ToString());
            orbitParams.Add("Semi Major Axis", _sgp.Orbit.SemiMajorAxis.ToString());
            orbitParams.Add("Recovered Semi Major Axis", _sgp.Orbit.RecoveredSemiMajorAxis.ToString());

            return orbitParams;
        }
    }
}
