using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface ISessionService
    {
        Task<bool> IsLoggedIn(string sessionId);
        Task<int?> UidLoggedIn(string sessionId);
        Task EndSession(string sessionId);
        Task<string?> UsernameLoggedIn(string sessionId);
    }
}
