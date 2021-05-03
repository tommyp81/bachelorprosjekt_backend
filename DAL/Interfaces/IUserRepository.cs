﻿using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> AddUser(AuthUser user);
        Task<User> UpdateUser(AuthUser user);
        Task<User> DeleteUser(int id);
        Task<IEnumerable<User>> PagedList(int page, int size, string order, string type);
    }
}
