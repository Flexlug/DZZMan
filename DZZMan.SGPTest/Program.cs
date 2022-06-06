using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SGPdotNET.Util;

var tle = new Tle(
    "SCD 1",
    "1 22490U 93009B   22154.14891900  .00000288  00000+0  29598-4 0  9998",
    "2 22490  24.9701 304.0131 0043053  25.1080 135.9398 14.44689799547482");


// Create a satellite from the TLEs
var sat = new Satellite(tle);

var sgp4 = new Sgp4(tle);

var periodMinites = sgp4.Orbit.Period;
var d = periodMinites / 200;

var periodInSpan = TimeSpan.FromMinutes(periodMinites);

var cords = new List<GeodeticCoordinate>();
for (double i = 0; i < periodMinites * 2; i += d)
    cords.Add(sgp4.FindPosition(i).ToGeodetic());

var sr = new StreamWriter("cords.txt");
foreach (var c in cords)
    sr.WriteLine($"{c.Latitude} - {c.Longitude} - {c.Altitude}");
sr.Flush();
sr.Dispose();

Console.ReadKey();