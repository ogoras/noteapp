using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    interface ILoginRepository
    {
        Task CreateAsync(Login l);
        Task<Login> ReadAsync(int id);
        Task UpdateAsync(Login l);
        Task DelAsync(Login l);
        Task<IEnumerable<Login>> ReadAllAsync();
        Task<IEnumerable<Login>> ReadAllAsync(int uid);
        Task<Login> ReadLastLogin(int uid);
    }
}
