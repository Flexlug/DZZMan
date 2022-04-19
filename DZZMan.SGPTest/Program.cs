using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SGPdotNET.Util;

// Remote URL
var url = new Uri("https://celestrak.com/NORAD/elements/weather.txt");

// Create a provider whose cache is renewed every 12 hours
var provider = new CachingRemoteTleProvider(true, TimeSpan.FromHours(12), "resource.txt", url);

// Get every TLE
var tles = provider.GetTles();

// Alternatively get a specific satellite's TLE
var issTle = provider.GetTle(25544);

// Create a satellite from the TLEs
var sat = new Satellite(issTle);

var sgp4 = new Sgp4(issTle);

