﻿using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserBLL
    {
        UserDTO AddDTO(User user);
        Task<ICollection<UserDTO>> GetUsers();
        Task<UserDTO> GetUser(int id);
        Task<UserDTO> AddUser(AuthUser user);
        Task<UserDTO> UpdateUser(AuthUser user);
        Task<UserDTO> DeleteUser(int id);
    }
}
