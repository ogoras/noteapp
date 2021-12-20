using Core.Domain;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    class NoteRepository : INoteRepository
    {
        public Task CreateAsync(Note n)
        {
            throw new NotImplementedException();
        }

        public Task DelAsync(Note n)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> ReadAllAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> ReadAllEncryptedAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> ReadAllSharedByAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Note>> ReadAllSharedToAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<Note> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Note> ReadDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Note n)
        {
            throw new NotImplementedException();
        }
    }
}
