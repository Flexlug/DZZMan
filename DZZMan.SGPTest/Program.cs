using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SGPdotNET.Util;
using SunCalcNet;

// var tle = new Tle(
//     "LANDSAT 7",
//     "1 25682U 99020A   22157.12066199  .00000233  00000+0  58054-4 0  9997",
//     "2 25682  97.9789 205.6187 0001656  97.1530 262.9865 14.59749584230915");
//
//
// // Create a satellite from the TLEs
// var sat = new Satellite(tle);
//
// var sgp4 = new Sgp4(tle);
//
// var periodMinites = sgp4.Orbit.Period;
// var d = periodMinites / 200;
//
// var periodInSpan = TimeSpan.FromMinutes(periodMinites);
//
// var cords = new List<GeodeticCoordinate>();
// for (double i = 0; i < periodMinites * 2; i += d)
//     cords.Add(sgp4.FindPosition(i).ToGeodetic());
//
// var sr = new StreamWriter("cords.txt");
// foreach (var c in cords)
//     sr.WriteLine($"{c.Latitude} - {c.Longitude} - {c.Altitude}");
// sr.Flush();
// sr.Dispose();
//
// Console.ReadKey();

double lat = -90;
double lon = -180;

double lastDarkLat = -90;

double deltaLat = 1;
double deltaLon = 1;


var sr = new StreamWriter("data.txt");

var sun = SunCalc.GetSunPhases(DateTime.Now, lat, lon);
sun = sun;
//
// do
// {
//     lat = lastDarkLat;
//     
//     do
//     {
//         lat += deltaLat;
//         var cel = SunCalc.GetSunPosition(DateTime.Now, lat, lon);
//         if (!cel.)
//             lastDarkLat = lat;
//     } while (!cel.IsSunUp);
//
//     lon += deltaLon;
//     
//     sr.WriteLine($"{lat} - {lon}");
// } while (lon <= 180);
//
//
// sr.Flush();
// sr.Close();