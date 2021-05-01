using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users != null)
            {
                return users;
            }
            else
            {
                return null;
            }
        }

        // GET: Users/1
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        // POST: Users
        public async Task<User> AddUser(AuthUser user)
        {
            // Sjekke om brukernavn eller epost eksisterer først
            var users = await _context.Users.ToArrayAsync();
            if (users != null)
            {
                foreach (var existingUser in users)
                {
                    if (existingUser.Username == user.Username || existingUser.Email == user.Email)
                    {
                        // Returnerer null hvis bruker eller epost finnes fra før
                        return null;
                    }
                }
            }

            // Krypter passord
            byte[] passwordSalt = AddSalt();
            byte[] passwordHash = AddHash(user.Password, passwordSalt);

            // Lage en ny bruker
            var newUser = new User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Admin = false,
                Password = passwordHash,
                Salt = passwordSalt
            };

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: Users/1
        public async Task<User> UpdateUser(AuthUser user)
        {
            var result = await _context.Users.FindAsync(user.Id);
            if (result != null)
            {
                // Sjekke om brukernavn eksisterer først
                var users = await _context.Users.ToArrayAsync();
                if (users != null)
                {
                    foreach (var existingUser in users)
                    {
                        // Se etter samme brukernavn og epost hvis ID ikke er lik
                        if (existingUser.Id != user.Id && (existingUser.Username == user.Username || existingUser.Email == user.Email))
                        {
                            // Returnerer null hvis bruker eller epost finnes fra før
                            return null;
                        }
                    }
                }

                // Krypter passord
                byte[] passwordSalt = AddSalt();
                byte[] passwordHash = AddHash(user.Password, passwordSalt);

                result.Id = result.Id;
                result.Username = user.Username;
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Email = user.Email;
                result.Admin = user.Admin;
                result.Password = passwordHash;
                result.Salt = passwordSalt;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        // DELETE: Users/1
        public async Task<User> DeleteUser(int id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result != null)
            {
                // Har denne brukeren har likes, må userId settes til null
                var likes = await _context.Likes.ToListAsync();
                if (likes != null)
                {
                    foreach (var like in likes)
                    {
                        if (like.UserId == result.Id)
                        {
                            like.UserId = null;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public static byte[] AddHash(string password, byte[] salt)
        {
            const int keyLength = 24;
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            return pbkdf2.GetBytes(keyLength);
        }

        public static byte[] AddSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }
    }
}
