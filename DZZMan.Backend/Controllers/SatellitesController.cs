using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DZZMan.Backend.Database.Providers;
using DZZMan.Models;
using Microsoft.Extensions.Primitives;

namespace DZZMan.Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SatellitesController : ControllerBase
    {
        private readonly ILogger<SatellitesController> _logger;
        private readonly SatelliteProvider _satelliteProvider;
        private readonly TokenProvider _tokenProvider;

        public SatellitesController(
            ILogger<SatellitesController> logger,
            SatelliteProvider satelliteProvider,
            TokenProvider tokenProvider)
        {
            _logger = logger;
            _satelliteProvider = satelliteProvider;
            _tokenProvider = tokenProvider;
        }

        [HttpGet(Name = "GetAllSatellites")]
        public async Task<ActionResult> GetAllAsync()
        {
            var res = await _satelliteProvider.GetSatellitesAsync();
            if (res is null || res.Count == 0)
                return NotFound();

            return new JsonResult(res);
        }

        [HttpGet(Name = "GetSatellite")]
        public async Task<ActionResult> GetAsync([FromQuery] string name)
        {
            var res = await _satelliteProvider.GetSatelliteAsync(name);

            if (res is null)
                return NotFound();

            return new JsonResult(res);
        }



        [HttpPut(Name = "AddSatellite")]
        public async Task<IActionResult> PutAsync(
            [FromBody] Satellite satellite)
        {
            if (!await ValidateAuth(Request))
                return Unauthorized();

            if (await _satelliteProvider.GetSatelliteAsync(satellite.Name) is not null)
                return Conflict("This object already exists");

            await _satelliteProvider.AddSatelliteAsync(satellite);
            return Ok();
        }

        [HttpPost(Name = "UpdateSatellite")]
        public async Task<ActionResult> UpdateAsync(
            [FromBody] Satellite satellite)
        {
            if (!await ValidateAuth(Request))
                return Unauthorized();

            if (await _satelliteProvider.GetSatelliteAsync(satellite.Name) is null)
                return NotFound();

            await _satelliteProvider.UpdateSatelliteAsync(satellite.Name, satellite);
            return Ok();
        }

        [HttpDelete(Name = "DeleteSatellite")]
        public async Task<ActionResult> DeleteAsync(
            [FromQuery] string name)
        {
            if (!await ValidateAuth(Request))
                return Unauthorized();

            if (await _satelliteProvider.GetSatelliteAsync(name) is null)
                return NotFound();

            await _satelliteProvider.DeleteSatelliteAsync(name);
            return Ok();
        }

        private async Task<bool> ValidateAuth(HttpRequest request)
        {
            KeyValuePair<string, StringValues> dummyKeyValue = new();
            var header = request.Headers.FirstOrDefault(x => x.Key == "DZZ-Auth", dummyKeyValue);
            
            if (header.Equals(dummyKeyValue))
                return false;

            if (!await _tokenProvider.ValidateTokenAsync(header.Value))
                return false;

            return true;
        }
    }
}
