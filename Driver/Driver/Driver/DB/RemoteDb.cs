﻿using Driver.API;
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
        private bool _disposed = false;
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

        ~RemoteDb()
        {
            Dispose(false);
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PostAsync("auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(content);
            return loginResponse;
        }

        public async Task<SignupResponse> SignUp(SignupRequest request)
        {
            SetToken(null);
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PostAsync("auth/signup", new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var signupResponse = JsonConvert.DeserializeObject<SignupResponse>(content);
            return signupResponse;
        }

        public async Task<AddDriveResponse> AddDrive(AddDriveRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PutAsync("drive", new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var addDriveReponse = JsonConvert.DeserializeObject<AddDriveResponse>(content);
            return addDriveReponse;
        }

        public async Task<GetDriveResponse> GetDrive(GetDriveRequest request)
        {
            var res = await _client.GetAsync($"drive/{request.DriveId}");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var getDriveResponse = JsonConvert.DeserializeObject<GetDriveResponse>(content);
            return getDriveResponse;
        }

        public async Task<DeleteDriveResponse> DeleteDrive(DeleteDriveRequest request)
        {
            var res = await _client.DeleteAsync($"drive/{request.DriveId}");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var deleteDriveReponse = JsonConvert.DeserializeObject<DeleteDriveResponse>(content);
            return deleteDriveReponse;
        }

        public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
        {
            HttpResponseMessage res = await _client.GetAsync($"person/{request.Username}");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var getPersonResponse = JsonConvert.DeserializeObject<GetPersonResponse>(content);
            return getPersonResponse;
        }

        public async Task<GetPersonDrivesResponse> GetPersonDrives(GetPersonDrivesRequest request)
        {
            var res = await _client.GetAsync($"person/{request.Username}/drives");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var getPersonDrivesReponse = JsonConvert.DeserializeObject<GetPersonDrivesResponse>(content);
            return getPersonDrivesReponse;
        }

        public async Task<GetPersonFriendsResponse> GetPersonFriends(GetPersonFriendsRequest request)
        {
            var res = await _client.GetAsync($"person/{request.Username}/friends");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var getPersonFriendsReponse = JsonConvert.DeserializeObject<GetPersonFriendsResponse>(content);
            return getPersonFriendsReponse;
        }

        public async Task<SearchPersonResponse> SearchPerson(SearchPersonRequest request)
        {
            var res = await _client.GetAsync($"person/{request.Query}/search");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var searchPersonResponse = JsonConvert.DeserializeObject<SearchPersonResponse>(content);
            return searchPersonResponse;
        }

        public async Task<AddFriendResponse> AddFriend(AddFriendRequest request)
        {
            string json = JsonConvert.SerializeObject(request, _settings);
            HttpResponseMessage res = await _client.PutAsync($"person/{request.Username}/friend", new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var addFriendResponse = JsonConvert.DeserializeObject<AddFriendResponse>(content);
            return addFriendResponse;
        }

        public async Task<DeleteFriendResponse> DeleteFriend(DeleteFriendRequest request)
        {
            var res = await _client.DeleteAsync($"person/{request.Username}/friend");
            var content = await res.Content.ReadAsStringAsync();
            EnsureSuccessStatusCode(res, content);

            var deleteFriendReponse = JsonConvert.DeserializeObject<DeleteFriendResponse>(content);
            return deleteFriendReponse;
        }

        public void SetToken(string token)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
            }

            if (token == null)
            {
                return;
            }

            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }

        private void EnsureSuccessStatusCode(HttpResponseMessage res, string content)
        {
            try
            {
                res.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new Exception(content);
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