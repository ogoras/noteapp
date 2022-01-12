using Core.Domain;
using Core.Repositories;
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

        public Task DeleteAsync(Session s)
        {
            throw new NotImplementedException();
        }

        public Task<Session?> ReadAsync(Guid id)
        {
            throw new NotImplementedException();
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

        public Task UpdateAsync(Session s)
        {
            throw new NotImplementedException();
        }
    }
}
