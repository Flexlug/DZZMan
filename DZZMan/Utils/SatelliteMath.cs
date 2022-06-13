using System;
using System.Collections.Generic;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;

namespace DZZMan.Utils;

public static class SatelliteMath
{
    public static List<GeodeticCoordinate> CalculateTrace(Tle tle, DateTime startDate, DateTime endDate, int density)
    {
        var sgp = new Sgp4(tle);
        
        var trace = new List<GeodeticCoordinate>()
        {
            new()
        };

        var traceTime = endDate - startDate;
        var timeDelta = default(TimeSpan);

        var period = TimeSpan.FromMinutes(sgp.Orbit.Period);

        var periodCount = traceTime / period;
        if (periodCount > 2)
        {
            timeDelta = (traceTime / periodCount) / density;
        }
        else
        {
            timeDelta = traceTime / density;
        }

        var iterCount = traceTime / timeDelta;
        var i = startDate;

        for (int ii = 0; ii < iterCount; i += timeDelta, ii++)
        {
            var geodeticCoordinate = sgp.FindPosition(i).ToGeodetic();
            trace.Add(geodeticCoordinate);
        }

        return trace;
    }

    public static List<GeodeticCoordinate> CalculateFoorptintBounds(Tle tle, DateTime date, int density)
    {
        var sgp = new Sgp4(tle);
        var position = sgp.FindPosition(date); 
        var boundCoordinates = position.GetFootprintBoundary(density);

        return boundCoordinates;
    }

    public static GeodeticCoordinate GetSatellitePosition(Tle tle, DateTime date)
    {
        var sgp = new Sgp4(tle);
        var position = sgp.FindPosition(date);
        var geodPosition = position.ToGeodetic();
        
        return geodPosition;
    }
}