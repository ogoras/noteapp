using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User u);
        Task<User?> ReadAsync(int id);
        Task<User?> ReadAsync(String username);
        Task UpdateAsync(User u);
        Task DeleteAsync(User u);
        Task<IEnumerable<User>> ReadAllAsync();
        Task<User?> ReadAsyncByEmail(string? email);
    }
}
