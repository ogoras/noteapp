using Core.Domain;
using Core.Repositories;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }
        public async Task Create(UserDTO u)
        {
            if (await _userRepository.ReadAsync(u.Username) != null)
                throw new ArgumentException("User already exists");
            User user = new User
            {
                Username = u.Username,
                Email = u.Email,
                Password = CalculatePasswordHash(u.Password),
                DateCreated = DateTime.Now,
                UserLogins = new List<Login>()
            };
            await _userRepository.CreateAsync(user);
            Profile profile = new Profile
            {
                User = user,
                UserId = (await _userRepository.ReadAsync(u.Username)).Uid,
                Notes = new List<Note>(),
                NoteShares = new List<Note>(),
                Photos = new List<Photo>(),
                PhotoShares = new List<Photo>()
            };
            await _profileRepository.CreateAsync(profile);
            user.Profile = profile;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<UserDTO>> ReadAll()
        {
            var users = await _userRepository.ReadAllAsync();

            return users.Select(x => new UserDTO(x));
        }

        private string CalculatePasswordHash(string password)
        {
            // TODO : proper hash calculation
            return password;
        }
    }
}
