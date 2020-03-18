using Driver.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Driver.DB
{
    public class RemoteDb : IDb
    {
        private HttpClient _client;
        private JsonSerializerSettings _settings;

        public RemoteDb(string remoteIp)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(remoteIp);
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
                Formatting = Formatting.Indented
            };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            var res = await _client.PostAsync("/auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(content);
        }

        public async Task<SignupResponse> SignUp(SignupRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            var res = await _client.PostAsync("auth/signup", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SignupResponse>(content);
        }

        public async Task<AddDriveResponse> AddDrive(AddDriveRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            var res = await _client.PutAsync("/drive", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AddDriveResponse>(content);
        }

        public async Task<GetDriveResponse> GetDrive(GetDriveRequest request)
        {
            var res = await _client.GetAsync($"/drive/{request.DriveId}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetDriveResponse>(content);
        }

        public async Task<DeleteDriveResponse> DeleteDrive(DeleteDriveRequest request)
        {
            var res = await _client.DeleteAsync($"/drive/{request.DriveId}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeleteDriveResponse>(content);
        }

        public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
        {
            var res = await _client.GetAsync($"/person/{request.Username}");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetPersonResponse>(content);
        }

        public async Task<GetPersonDrivesResponse> GetPersonDrives(GetPersonDrivesRequest request)
        {
            var res = await _client.GetAsync($"/person/{request.Username}/drives");
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetPersonDrivesResponse>(content);
        }
    }

    public class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}