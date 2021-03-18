using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: Users/1
        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // POST: Users
        public async Task<User> AddUser(User user)
        {
            var result = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: Users/1
        public async Task<User> UpdateUser(User user)
        {
            var result = await _context.Users.FindAsync(user.Id);
            if (result != null)
            {
                result.Id = user.Id;
                result.Username = user.Username;
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Password = user.Password;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: Users/1
        public async Task<User> DeleteUser(int id)
        {
            var result = await _context.Users.FindAsync(id);
            if (result != null)
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
