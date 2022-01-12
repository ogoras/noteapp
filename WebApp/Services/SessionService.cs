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
                if (apiResponse == null)
                    return false;
            }

            return true;
        }
    }
}
