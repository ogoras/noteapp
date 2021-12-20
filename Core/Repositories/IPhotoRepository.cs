using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IPhotoRepository
    {
        Task CreateAsync(Photo p);
        Task<Photo> ReadAsync(int id);
        Task UpdateAsync(Photo p);
        Task DelAsync(Photo p);
        Task<IEnumerable<Photo>> ReadAllAsync();
        Task<IEnumerable<Photo>> ReadAllAsync(int uid);
        Task<Photo> ReadAttachedAsync(int nid);
        Task<Photo> ReadProfileAsync(int uid);
        Task<IEnumerable<Photo>> ReadPhotosSharedToAsync(int uid);
        Task<IEnumerable<Photo>> ReadPhotosSharedByAsync(int uid);
    }
}
