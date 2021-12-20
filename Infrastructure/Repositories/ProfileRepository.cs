using Core.Domain;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public ProfileRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            dbSet = appDbContext.Profiles;
        }

        public async Task DelAsync(Profile p)
        {
            await base.DelAsync(p, x => x.Id == p.Id);
        }

        public Task<IEnumerable<Profile>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Profile> ReadAsync(int uid)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Profile p)
        {
            throw new NotImplementedException();
        }
    }
}
