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
        Task<IEnumerable<UserDTOwithID>> ReadAll();
        Task<UserDTO?> Read(string username);
        Task Update(int id, UserDTO user);
        Task Delete(int id);
        Task<string?> Login(LoginDTO login);
    }
}
