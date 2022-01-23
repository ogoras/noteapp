using Core.Domain;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            dbSet = appDbContext.Sessions;
        }

        public async Task DeleteAsync(Session s)
        {
            await base.DeleteAsync(x => x.Id == s.Id);
        }

        public async Task<Session?> ReadAsync(Guid id)
        {
            return await base.ReadAsync(dbSet.Include(session => session.User),
                x => x.Id == id);
        }

        public Task<Session?> ReadAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Session>> ReadByUserAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Session>> ReadByUserAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Session s)
        {
            await base.UpdateAsync(s, x => x.Id == s.Id);
        }
    }
}
