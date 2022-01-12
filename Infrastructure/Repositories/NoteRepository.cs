using Core.Domain;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            dbSet = appDbContext.Notes;
        }
        public async Task DeleteAsync(Note n)
        {
            await base.DeleteAsync(x => x.Id == n.Id);
        }

        public async Task<IEnumerable<Note>> ReadAllAsync(int uid)
        {
            return await base.ReadAllAsync(dbSet.Include(note => note.Owner)
                .ThenInclude(profile => profile.User),
                x => x.Owner.User.Uid == uid);
        }

        public async Task<IEnumerable<Note>> ReadAllEncryptedAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> ReadAllSharedByAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> ReadAllSharedToAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public async Task<Note> ReadAsync(int id)
        {
            return await base.ReadAsync(x => x.Id == id);
        }

        public async Task<Note> ReadAsyncWithOwner(int id)
        {
            return await base.ReadAsync(dbSet.Include(n=>n.Owner), x => x.Id == id);
        }

        public async Task<Note> ReadDetailsAsync(int id)
        {
            return await base.ReadAsync(dbSet.Include(note => note.Owner)
                .Include(note => note.AttachedPhotos)
                .Include(note => note.ShareRecipients),
                x => x.Id == id);
        }

        public async Task UpdateAsync(Note n)
        {
            await base.UpdateAsync(n, x => x.Id == n.Id);
        }
    }
}
