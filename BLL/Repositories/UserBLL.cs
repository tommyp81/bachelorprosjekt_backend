using BLL.Interfaces;
using DAL.Helpers;
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

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _repository.GetUsers();
            if (users != null)
            {
                var userDTOs = new List<UserDTO>();
                foreach (User user in users)
                {
                    userDTOs.Add(new UserDTO(user));
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
                return new UserDTO(getUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> AddUser(NewUser user)
        {
            var addUser = await _repository.AddUser(user);
            if (addUser != null)
            {
                return new UserDTO(addUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> UpdateUser(NewUser user)
        {
            var updateUser = await _repository.UpdateUser(user);
            if (updateUser != null)
            {
                return new UserDTO(updateUser);
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
                return new UserDTO(deleteUser);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<UserDTO>>> PagedList(int page, int size, string order, string type)
        {
            var users = await _repository.PagedList(page, size, order, type);
            if (users != null)
            {
                var userDTOs = new List<UserDTO>();
                foreach (var user in users.Data)
                {
                    userDTOs.Add(new UserDTO(user));
                }
                return new PageResponse<IEnumerable<UserDTO>>(userDTOs, users.Count, null, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<UserDTO>>(null, 0, null, page, size, order, type);
            }
        }

        public async Task<PageResponse<IEnumerable<UserDTO>>> Search(string query, int page, int size, string order, string type)
        {
            var users = await _repository.Search(query, page, size, order, type);
            if(users != null)
            {
                var userDTOs = new List<UserDTO>();
                foreach (var user in users.Data)
                {
                    userDTOs.Add(new UserDTO(user));
                }
                return new PageResponse<IEnumerable<UserDTO>>(userDTOs, users.Count, null, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<UserDTO>>(null, 0, null, page, size, order, type);
            }
        }
    }
}
