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
            await base.DeleteAsync(x => x.Id == p.Id);
        }

        public async Task<Profile> ReadAsync(int uid)
        {
            return await base.ReadAsync(p => p.UserId == uid);
        }

        public async Task UpdateAsync(Profile p)
        {
            await base.UpdateAsync(p, x => x.Id == p.Id);
        }
    }
}
