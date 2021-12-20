using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        Task Create(UserDTO u);
        Task<IEnumerable<UserDTO>> ReadAll();
    }
}
