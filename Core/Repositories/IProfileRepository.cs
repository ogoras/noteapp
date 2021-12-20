using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IProfileRepository
    {
        Task CreateAsync(Profile p);
        Task<Profile> ReadAsync(int uid);
        Task UpdateAsync(Profile p);
        Task DelAsync(Profile p);
        Task<IEnumerable<Profile>> ReadAllAsync();
    }
}
