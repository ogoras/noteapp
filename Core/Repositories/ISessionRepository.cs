using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface ISessionRepository
    {
        Task CreateAsync(Session s);
        Task<Session?> ReadAsync(Guid id);
        Task<Session?> ReadAsync(string id);
        Task UpdateAsync(Session s);
        Task DeleteAsync(Session s);
        Task<IEnumerable<Session>> ReadAllAsync();
        Task<IEnumerable<Session>> ReadByUserAsync(int uid);
        Task<IEnumerable<Session>> ReadByUserAsync(string username);
    }
}
