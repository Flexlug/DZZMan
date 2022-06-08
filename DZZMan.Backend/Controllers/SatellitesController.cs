using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DZZMan.Backend.Database.Providers;
using DZZMan.Models;
using Microsoft.Extensions.Primitives;

namespace DZZMan.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet(template: "GetAll", Name = "GetAllSatellites")]
        public async Task<ActionResult> GetAllAsync()
        {
            var res = await _satelliteProvider.GetSatellitesAsync();

            return new JsonResult(res);
        }

        [HttpGet(template: "Get", Name = "GetSatellite")]
        public async Task<ActionResult> GetAsync([FromQuery] string name)
        {
            var res = await _satelliteProvider.GetSatelliteAsync(name);

            if (res is null)
                return NotFound();

            return new JsonResult(res);
        }

        [HttpPut(template: "Add", Name = "AddSatellite")]
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

        [HttpPost(template: "Update", Name = "UpdateSatellite")]
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

        [HttpDelete(template: "Delete", Name = "DeleteSatellite")]
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

        [HttpPost(template: "ValidateToken", Name = "ValidateToken")]
        public async Task<ActionResult> ValidateTokenAsync()
        {
            if (!await ValidateAuth(Request))
                return Unauthorized();

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
