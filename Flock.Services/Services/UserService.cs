using Flock.Common.Domain;
using Flock.Common.Exceptions;
using Flock.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Flock.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> logger;
        public UserService(ILogger<UserService> logger)
        {
            this.logger = logger;
        }

        private static User[] FakeUserDb()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
            var json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<User[]>(json);
            return users;
        }

        public User GetById(int id)
        {
            return FakeUserDb().SingleOrDefault(x => x.Id == id);
        }

        public User Login(string username, string password)
        {
            var user = FakeUserDb().SingleOrDefault(x => x.Username == username);

            if (user == null)
            {
                throw new ValidationException($"El usuario {username} no existe");
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ValidationException("La contraseña es incorrecta");
            }

            return user;
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
