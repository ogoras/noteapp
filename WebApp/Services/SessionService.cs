using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public class SessionService : ISessionService
    {
        public IConfiguration Configuration;
        private string _endpointUrl;

        public SessionService(IConfiguration configuration)
        {
            Configuration = configuration;
            _endpointUrl = Configuration["RestApiUrl"] + "user";
        }

        public async Task EndSession(string sessionId)
        {
            await new HttpClient().DeleteAsync(_endpointUrl + $"/session/{sessionId}");
        }

        public async Task<bool> IsLoggedIn(string? sessionId)
        {
            if (sessionId == null)
                return false;

            using (var response = await new HttpClient().GetAsync(_endpointUrl + $"/bysession/{sessionId}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (apiResponse == null || apiResponse == "null")
                    return false;
            }

            return true;
        }

        public async Task<int?> UidLoggedIn(string sessionId)
        {
            string? username = await UsernameLoggedIn(sessionId);
            if (username == null)
                return null;

            using (var response = await new HttpClient().GetAsync(_endpointUrl + $"/username/{username}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                IdObject user = JsonConvert.DeserializeObject<IdObject>(apiResponse);
                return user.Id;
            }
        }

        public async Task<string?> UsernameLoggedIn(string sessionId)
        {
            if (!await IsLoggedIn(sessionId))
                return null;

            string username;

            using (var response = await new HttpClient().GetAsync(_endpointUrl + $"/bysession/{sessionId}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                username = JsonConvert.DeserializeObject<string?>(apiResponse);
            }

            return username;
        }

        public async Task<string?> Role(string username)
        {
            string? role;

            using (var response = await new HttpClient().GetAsync(_endpointUrl + $"/role/{username}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                role = JsonConvert.DeserializeObject<string?>(apiResponse) == "null" ? null : JsonConvert.DeserializeObject<string?>(apiResponse);
            }

            return role;
        }

        private class IdObject
        {
            public int Id { get; set; }
        }
    }
}
