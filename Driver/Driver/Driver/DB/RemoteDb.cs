using Driver.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Driver.DB
{
    public class RemoteDb : IDb
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        public RemoteDb(string remoteIp)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(remoteIp)
            };
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver(),
                Formatting = Formatting.Indented
            };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PostAsync("auth/login", new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            string token = GetFirstInstance<string>("token", content);
            _client.DefaultRequestHeaders.Add("Authorization", token);
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);
            return loginResponse;
        }

        public async Task<SignupResponse> SignUp(SignupRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PostAsync("auth/signup", new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var signupResponse = JsonConvert.DeserializeObject<SignupResponse>(content);
            return signupResponse;
        }

        public async Task<AddDriveResponse> AddDrive(AddDriveRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PutAsync("drive", new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var addDriveReponse = JsonConvert.DeserializeObject<AddDriveResponse>(content);
            return addDriveReponse;
        }

        public async Task<GetDriveResponse> GetDrive(GetDriveRequest request)
        {
            var res = await _client.GetAsync($"drive/{request.DriveId}").ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var getDriveResponse = JsonConvert.DeserializeObject<GetDriveResponse>(content);
            return getDriveResponse;
        }

        public async Task<DeleteDriveResponse> DeleteDrive(DeleteDriveRequest request)
        {
            var res = await _client.DeleteAsync($"drive/{request.DriveId}").ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var deleteDriveReponse = JsonConvert.DeserializeObject<DeleteDriveResponse>(content);
            return deleteDriveReponse;
        }

        public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
        {
            HttpResponseMessage res = await _client.GetAsync($"person/{request.Username}").ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var getPersonResponse = JsonConvert.DeserializeObject<GetPersonResponse>(content);
            return getPersonResponse;
        }

        public async Task<GetPersonDrivesResponse> GetPersonDrives(GetPersonDrivesRequest request)
        {
            var res = await _client.GetAsync($"person/{request.Username}/drives").ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var getPersonDrivesReponse = JsonConvert.DeserializeObject<GetPersonDrivesResponse>(content);
            return getPersonDrivesReponse;
        }

        public async Task<GetPersonFriendsResponse> GetPersonFriends(GetPersonFriendsRequest request)
        {
            var res = await _client.GetAsync($"person/{request.Username}/friends").ConfigureAwait(false);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.EnsureSuccessStatusCode();

            var getPersonFriendsReponse = JsonConvert.DeserializeObject<GetPersonFriendsResponse>(content);
            return getPersonFriendsReponse;
        }

        private T GetFirstInstance<T>(string propertyName, string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName
                        && (string)jsonReader.Value == propertyName)
                    {
                        jsonReader.Read();

                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
                throw new Exception("No token received from server");
            }
        }

        private class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            {
                JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

                contract.DictionaryKeyResolver = propertyName => propertyName;

                return contract;
            }
        }
    }
}