using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface INoteRepository
    {
        Task CreateAsync(Note n);
        Task<Note> ReadAsync(int id);
        Task<Note> ReadAsyncWithOwner(int id);
        Task<Note> ReadDetailsAsync(int id);
        Task UpdateAsync(Note n);
        Task DeleteAsync(Note n);
        Task<IEnumerable<Note>> ReadAllAsync();
        Task<IEnumerable<Note>> ReadAllAsync(int uid);
        Task<IEnumerable<Note>> ReadAllSharedToAsync(int uid);
        Task<IEnumerable<Note>> ReadAllSharedByAsync(int uid);
        Task<IEnumerable<Note>> ReadAllEncryptedAsync(int uid);
        Task<IEnumerable<Note>> ReadPublicAsync();
    }
}
