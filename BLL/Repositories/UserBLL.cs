﻿using BLL.Interfaces;
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

        public UserBLL(IUserRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for Users
        public UserDTO AddDTO(User user)
        {
            UserDTO DTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password
            };
            return DTO;
        }

        public async Task<ICollection<UserDTO>> GetUsers()
        {
            ICollection<User> users = await _repository.GetUsers();
            if (users == null) { return null; }
            ICollection<UserDTO> userDTOs = new List<UserDTO>();
            foreach (User user in users)
            {
                User getUser = await _repository.GetUser(user.Id);
                if (getUser == null) { return null; }
                userDTOs.Add(AddDTO(getUser));
            }
            return userDTOs;
        }

        public async Task<UserDTO> GetUser(int id)
        {
            User getUser = await _repository.GetUser(id);
            if (getUser == null) { return null; }
            UserDTO userDTO = AddDTO(getUser);
            return userDTO;
        }

        public async Task<UserDTO> AddUser(User user)
        {
            User addUser = await _repository.AddUser(user);
            if (addUser == null) { return null; }
            UserDTO userDTO = AddDTO(addUser);
            return userDTO;
        }

        public async Task<UserDTO> UpdateUser(User user)
        {
            User updateUser = await _repository.UpdateUser(user);
            if (updateUser == null) { return null; }
            UserDTO userDTO = AddDTO(updateUser);
            return userDTO;
        }

        public async Task<UserDTO> DeleteUser(int id)
        {
            User deleteUser = await _repository.DeleteUser(id);
            if (deleteUser == null) { return null; }
            UserDTO userDTO = AddDTO(deleteUser);
            return userDTO;
        }
    }
}
