using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using BruTile.Predefined;
using DZZMan.Models;
using DZZMan.Models.Sensors;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Implementation;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SunCalcNet;

namespace DZZMan.Utils;

public static class SatelliteMath
{
    public const int DEFAULT_TRACE_DENSITY = 200;
    public const int DEFAULT_FOOTPRINT_DINSITY = 120;
    
    public static List<EciCoordinate> CalculateTraceEci(Tle tle, DateTime startDate, DateTime endDate, 
        int density)
    {
        var sgp = new Sgp4(tle);
        
        var trace = new List<EciCoordinate>()
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
            var geodeticCoordinate = sgp.FindPosition(i);
            trace.Add(geodeticCoordinate);
        }

        return trace;
    }

    public static List<GeodeticCoordinate> CalculateTraceGeod(Tle tle, DateTime startDate, DateTime endDate,
        int density)
    {
        var eciCords = CalculateTraceEci(tle, startDate, endDate, density);
        var geodCords = eciCords.Select(x => x.ToGeodetic()).ToList();

        return geodCords;
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

    public static List<Polygon> CalculateCapturedAreaForScanner(Tle tle, Satellite satellite, DateTime startTime, DateTime endTime, bool skipDark)
    {
        var scanner = satellite.Sensor as Scanner;
        
        var trace = CalculateTraceEci(tle, startTime, endTime, DEFAULT_TRACE_DENSITY);

        var halfDist = scanner.Swath / 2;
        
        var prevPointEci = trace[0];
        var prevPointGeod = prevPointEci.ToGeodetic();
        var prevPoint = SphericalMercator.FromLonLat(prevPointGeod.Longitude.Degrees, prevPointGeod.Latitude.Degrees).ToCoordinate();

        EciCoordinate currentPointEci;
        GeodeticCoordinate currentPointGeod;
        
        var polygonPointSequenses = new List<List<NetTopologySuite.Geometries.Coordinate>>();
        var polygonIterators = 0;
        var isSkiping = false;
        
        for (int i = 1; i < trace.Count - 1; i++)
        {
            currentPointEci = trace[i];
            currentPointGeod = currentPointEci.ToGeodetic();

            if (skipDark)
            {
                var sun = SunCalc.GetSunPosition(prevPointEci.Time, prevPointGeod.Latitude.Degrees, prevPointGeod.Longitude.Degrees);
                if (sun.Altitude <= 0)
                {
                    if (isSkiping)
                    {
                        continue;
                    }
                    
                    polygonPointSequenses.Add(new());
                    polygonIterators++;
                    
                    isSkiping = true;
                    continue;
                }
                isSkiping = false;
            }
            
            var currentPoint = SphericalMercator.FromLonLat(currentPointGeod.Longitude.Degrees, currentPointGeod.Latitude.Degrees).ToCoordinate();

            var angle = AngleUtility.Angle(prevPoint, currentPoint);

            var cRight = GeodesyMath.DirectGeodTask(prevPoint, angle + 90, halfDist);
            var cLeft = GeodesyMath.DirectGeodTask(prevPoint, angle - 90, halfDist);
            
            polygonPointSequenses[polygonIterators].Add(cRight);
            polygonPointSequenses[polygonIterators].Add(cLeft);

            prevPoint = currentPoint;
        }

        var geometryFactory = new GeometryFactory();
        var polygons = polygonPointSequenses
            .Select(x => x.ToArray())
            .Select(x => geometryFactory.CreatePolygon(x))
            .ToList();

        return polygons;
    }
}