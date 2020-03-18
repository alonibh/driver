using Driver.API;
using Driver.Dbo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Driver.DB
{
    public class RemoteDb : IDb
    {
        private HttpClient _client;

        public RemoteDb(string remoteIp)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(remoteIp);
        }

        public async Task<bool> Login(LoginRequest request)
        {
            string json = JsonConvert.SerializeObject(request);
            var res = await _client.PostAsync("/auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(content);
        }

        public async Task<bool> SignUp(SignupRequest request)
        {
            string json = JsonConvert.SerializeObject(request);
            var res = await _client.PostAsync("/auth/signup", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(content);
        }

        public async Task<bool> AddDrive(AddDriveRequest request)
        {
            string json = JsonConvert.SerializeObject(request);
            var res = await _client.PutAsync("/drive", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(content);
        }

        public async Task<DriveDbo> GetDrive(GetDriveRequest request)
        {
            var res = await _client.GetAsync($"/drive/{request.DriveId}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DriveDbo>(content);
        }

        public async Task<bool> DeleteDrive(DeleteDriveRequest request)
        {
            var res = await _client.DeleteAsync($"/drive/{request.DriveId}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(content);
        }

        public async Task<PersonDbo> GetPerson(GetPersonRequest request)
        {
            var res = await _client.GetAsync($"/person/{request.Username}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PersonDbo>(content);
        }

        public async Task<List<DriveDbo>> GetPersonDrives(GetPersonDrivesRequest request)
        {
            var res = await _client.GetAsync($"/person/{request.Username}/drives");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DriveDbo>>(content);
        }
    }
}