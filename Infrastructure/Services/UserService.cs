using Core.Domain;
using Core.Repositories;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                throw new ArgumentException("A user with the username " + u.Username + " already exists!");
            if (await _userRepository.ReadAsyncByEmail(u.Email) != null)
                throw new ArgumentException("A user with the email " + u.Email + " already exists! Did you mean to log in?");
            User user = new User
            {
                Username = u.Username,
                Email = u.Email,
                Password = generatePasswordHash(u.Password),
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

        public async Task<IEnumerable<UserDTOwithID>> ReadAll()
        {
            var users = await _userRepository.ReadAllAsync();

            return users.Select(x => new UserDTOwithID(x));
        }

        public async Task<UserDTO?> Read(string username)
        {
            User? u = await _userRepository.ReadAsync(username);
            return u == null ? null : new UserDTO(u);
        }

        private string? generatePasswordHash(string? password)
        {
            if (password == null)
                return null;

            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            string saltString = Convert.ToBase64String(salt);

            byte[] hash = calculateHash(password, salt);
            string hashString = Convert.ToBase64String(hash);

            return $"{saltString}${hashString}";
        }

        private static byte[] calculateHash(string password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
            new byte[password.Length + salt.Length];

            for (int i = 0; i < password.Length; i++)
            {
                plainTextWithSaltBytes[i] = (byte)password[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }

            var hash = algorithm.ComputeHash(plainTextWithSaltBytes);
            return hash;
        }

        public async Task Update(int id, UserDTO user)
        {
            User? original = await _userRepository.ReadAsync(id);
            if (user.Username != null)
            {
                User? u = await _userRepository.ReadAsync(user.Username);
                if (u != null && u != original)
                    throw new ArgumentException("A user with the username " + user.Username + " already exists!");
            }
            if (user.Email != null)
            {
                User? u = await _userRepository.ReadAsyncByEmail(user.Email);
                if (u != null && u != original)
                    throw new ArgumentException("A user with the email " + user.Email + " already exists!");
            }
            User updated = new User()
            {
                Uid = id,
                Username = user.Username ?? original.Username,
                Email = user.Email ?? original.Email,
                Password = generatePasswordHash(user.Password) ?? original.Password
            };

            await _userRepository.UpdateAsync(updated);
        }

        public async Task Delete(int id)
        {
            User? u = await _userRepository.ReadAsync(id);
            if (u == null)
                throw new NullReferenceException();
            await _userRepository.DeleteAsync(u);
        }

        public async Task<string?> Login(LoginDTO login)
        {
            if (login.Username == null || login.Password == null)
                throw new NullReferenceException();
            User? u = await _userRepository.ReadAsync(login.Username);
            if (u == null)
                throw new NullReferenceException();
            var arr = u.Password.Split('$');
            var hash = calculateHash(login.Password, Convert.FromBase64String(arr[0]));
            return compareHashes(hash, Convert.FromBase64String(arr[1])) ? generateJWT(u) : null;
        }

        private bool compareHashes(byte[] hash, byte[] vs)
        {
            if (hash.Length != vs.Length)
                return false;
            for (int i = 0; i < hash.Length; i++)
                if (hash[i] != vs[i])
                    return false;
            return true;
        }

        private string generateJWT(User u)
        {
            throw new NotImplementedException();
        }
    }
}
