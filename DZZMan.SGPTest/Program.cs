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

var cords = new List<GeodeticCoordinate>();
for (int i = 0; i < 1000000; i += 100)
    cords.Add(sgp4.FindPosition(i).ToGeodetic());

Console.ReadKey();