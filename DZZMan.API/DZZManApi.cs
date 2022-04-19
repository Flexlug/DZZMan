using DZZMan.API.Exceptions;
using DZZMan.Models;

using RestSharp;

using Newtonsoft.Json;

namespace DZZMan.API
{
    public class DZZManApi
    {
        private string _IP;
        private RestClient client = new();

        private bool _hasToken;
        private string _token = string.Empty;

        public DZZManApi(string IP)
        {
            _IP = IP;

            _hasToken = false;
        }

        public async Task<List<Satellite>> GetSatellitesAsync()
        {
            RestRequest request = new($"{_IP}/Satellites/GetAll");

            var resp = await client.GetAsync(request);

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            List<Satellite> satellites = JsonConvert.DeserializeObject<List<Satellite>>(resp.Content); ;

            return satellites;
        }

        public async Task<Satellite> GetSatelliteAsync(string name)
        {
            RestRequest request = new($"{_IP}/Satellites/Get?name={name}");

            var resp = await client.GetAsync(request);

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSatellitesException();
            
            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            Satellite satellite = JsonConvert.DeserializeObject<Satellite>(resp.Content); ;

            return satellite;
        }

        public async Task LoginAsync(string token)
        {
            RestRequest request = new($"{_IP}/Satellites/ValidateToken");

            request.AddHeader("DZZ-Auth", token);

            var resp = await client.PostAsync(request);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            _hasToken = true;
            _token = token;
        }

        public async Task AddSatelliteAsync(Satellite satellite)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Add");

            request.AddJsonBody(satellite);
            request.AddHeader("DZZ-Auth", _token);

            var resp = await client.PutAsync(request);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.Conflict)
                throw new AlreadyExistsException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }

        public async Task UpdateSatelliteAsync(Satellite satellite)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Update");

            request.AddJsonBody(satellite);
            request.AddHeader("DZZ-Auth", _token);

            var resp = await client.PostAsync(request);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }

        public async Task DeleteSatelliteAsync(string name)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Delete?name={name}");

            request.AddHeader("DZZ-Auth", _token);

            var resp = await client.DeleteAsync(request);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }
        public List<Satellite> GetSatellites()
        {
            RestRequest request = new($"{_IP}/Satellites/GetAll");

            var resp = client.GetAsync(request).Result;

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            List<Satellite> satellites = JsonConvert.DeserializeObject<List<Satellite>>(resp.Content); ;

            return satellites;
        }

        public Satellite GetSatellite(string name)
        {
            RestRequest request = new($"{_IP}/Satellites/Get?name={name}");

            var resp = client.GetAsync(request).Result;

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NoSatellitesException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            Satellite satellite = JsonConvert.DeserializeObject<Satellite>(resp.Content); ;

            return satellite;
        }

        public void Login(string token)
        {
            RestRequest request = new($"{_IP}/Satellites/ValidateToken");

            request.AddHeader("DZZ-Auth", token);

            var resp = client.PostAsync(request).Result;

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");

            _hasToken = true;
            _token = token;
        }

        public void AddSatellite(Satellite satellite)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Add");

            request.AddJsonBody(satellite);
            request.AddHeader("DZZ-Auth", _token);

            var resp = client.PutAsync(request).Result;

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.Conflict)
                throw new AlreadyExistsException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }

        public void UpdateSatellite(Satellite satellite)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Update");

            request.AddJsonBody(satellite);
            request.AddHeader("DZZ-Auth", _token);

            var resp = client.PostAsync(request).Result;

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }

        public void DeleteSatellite(string name)
        {
            if (!_hasToken)
                throw new UnauthorizedAccessException("You did not logged in");

            RestRequest request = new($"{_IP}/Satellites/Delete?name={name}");

            request.AddHeader("DZZ-Auth", _token);

            var resp = client.DeleteAsync(request).Result;

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Wrong token");

            if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new KeyNotFoundException();

            if (!resp.IsSuccessful)
                throw new Exception($"Unrecognized exception: {resp.StatusCode}");
        }
    }
}