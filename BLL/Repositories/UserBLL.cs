using BLL.Interfaces;
using DAL.Interfaces;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserRepository _repository;

        public UserBLL(IUserRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Users
        public UserDTO AddDTO(User user)
        {
            var DTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Admin = user.Admin
            };
            return DTO;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _repository.GetUsers();
            if (users != null)
            {
                var userDTOs = new List<UserDTO>();
                foreach (User user in users)
                {
                    userDTOs.Add(AddDTO(user));
                }
                return userDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> GetUser(int id)
        {
            var getUser = await _repository.GetUser(id);
            if (getUser != null)
            {
                return AddDTO(getUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> AddUser(AuthUser user)
        {
            var addUser = await _repository.AddUser(user);
            if (addUser != null)
            {
                return AddDTO(addUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> UpdateUser(AuthUser user)
        {
            var updateUser = await _repository.UpdateUser(user);
            if (updateUser != null)
            {
                return AddDTO(updateUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> DeleteUser(int id)
        {
            var deleteUser = await _repository.DeleteUser(id);
            if (deleteUser != null)
            {
                return AddDTO(deleteUser);
            }
            else
            {
                return null;
            }
        }
    }
}
