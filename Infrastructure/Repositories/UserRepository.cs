using Core.Domain;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            dbSet = appDbContext.Users;
        }
        public async Task DeleteAsync(User u)
        {
            throw new NotImplementedException();
        }

        public async Task<User> ReadAsync(int id)
        {
            return await base.ReadAsync(u => u.Uid == id);
        }

        public async Task<User> ReadAsync(string username)
        {
            return await base.ReadAsync(x => x.Username == username);
        }

        public async Task UpdateAsync(User u)
        {
            await base.UpdateAsync(u, x => x.Uid == u.Uid);
        }
    }
}
