using Core.Domain;
using Core.Repositories;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;

        public UserService(IUserRepository userRepository, IProfileRepository profileRepository, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _sessionRepository = sessionRepository;
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
                UserLogins = new List<Login>(),
                Sessions = new List<Session>()
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

        public async Task<UserDTOwithID?> Read(string username)
        {
            User? u = await _userRepository.ReadAsync(username);
            return u == null ? null : new UserDTOwithID(u);
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

            if (u.LockoutTime?.AddMinutes(3).CompareTo(DateTime.Now) > 0)
                throw new UnauthorizedAccessException();
            else if (u.LockoutTime != null)
            {
                u.LockoutTime = null;
                await _userRepository.UpdateAsync(u);
            }

            var arr = u.Password.Split('$');
            var hash = calculateHash(login.Password, Convert.FromBase64String(arr[0]));
            if (compareHashes(hash, Convert.FromBase64String(arr[1])))
            {
                u.LoginAttemptsSinceLockout = 0;
                await _userRepository.UpdateAsync(u);
                return generateSession(u);
            }

            if (u.LoginAttemptsSinceLockout == 10)
            {
                u.LoginAttemptsSinceLockout = 0;
                u.LockoutTime = DateTime.Now;
                await _userRepository.UpdateAsync(u);
                throw new UnauthorizedAccessException();
            }

            u.LoginAttemptsSinceLockout = u.LoginAttemptsSinceLockout == null ? 1 : u.LoginAttemptsSinceLockout + 1;
            await _userRepository.UpdateAsync(u);
            await Task.Delay(3000);
            return null;
        }

        private string generateSession(User u)
        {
            Session s = new Session()
            {
                Id = Guid.NewGuid(),
                User = u,
                LastActivity = DateTime.Now
            };
            _sessionRepository.CreateAsync(s);
            return s.Id.ToString();
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"supersupersecret"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "noteapp",
                audience: "noteapp",
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials,
                claims: new[] {new Claim("username", u.Username)}
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> UsernameFromSession(Guid sessionid)
        {
            Session? s = await _sessionRepository.ReadAsync(sessionid);
            if (s?.LastActivity.AddHours(1).CompareTo(DateTime.Now) < 0)
            {
                await _sessionRepository.DeleteAsync(s);
                return null;
            }
            else if (s == null)
            {
                return null;
            }
            else
            {
                await ExtendSession(s.Id);
                return s.User.Username;
            }
        }

        public async Task EndSession(Guid id)
        {
            Session? s = await _sessionRepository.ReadAsync(id);
            if (s == null)
                throw new NullReferenceException();
            await _sessionRepository.DeleteAsync(s);
        }

        public async Task ExtendSession(Guid id)
        {
            Session? s = await _sessionRepository.ReadAsync(id);
            if (s == null)
                throw new NullReferenceException();
            s.LastActivity = DateTime.Now;
            await _sessionRepository.UpdateAsync(s);
        }

        public async Task<string?> GetRole(string username)
        {
            User? u = await _userRepository.ReadAsync(username);
            return u?.Role;
        }

        public async Task UpdatePassword(ChangePasswordDTO cp)
        {
            User? u = await _userRepository.ReadAsync(cp.Uid);

            if (u == null)
                throw new NullReferenceException();

            var arr = u.Password.Split('$');
            var hash = calculateHash(cp.CurrentPassword, Convert.FromBase64String(arr[0]));
            if (compareHashes(hash, Convert.FromBase64String(arr[1])))
            {
                u.Password = generatePasswordHash(cp.NewPassword);
                await _userRepository.UpdateAsync(u);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
