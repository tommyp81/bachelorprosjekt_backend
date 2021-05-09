using DAL.Helpers;
using Model.Auth;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class UserObject
    {
        public static PageResponse<IEnumerable<UserDTO>> TestPagedResponse(string query)
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "sysadmin",
                    FirstName = "Superbruker",
                    LastName = "NFB",
                    Email = "admin@badminton.no",
                    Admin = true
                },
                new User
                {
                    Id = 2,
                    Username = "user1",
                    FirstName = "Bruker 1",
                    LastName = "NFB",
                    Email = "user1@badminton.no",
                    Admin = true
                },
                new User
                {
                    Id = 3,
                    Username = "user2",
                    FirstName = "Bruker 2",
                    LastName = "NFB",
                    Email = "user2@badminton.no",
                    Admin = false
                },
            };

            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                userDTOs.Add(new UserDTO(user));
            }

            if (string.IsNullOrEmpty(query))
            {
                return new PageResponse<IEnumerable<UserDTO>>(userDTOs);
            }
            else
            {
                var searchResult = userDTOs.AsQueryable().Where(q => q.Username.Contains(query));
                return new PageResponse<IEnumerable<UserDTO>>(searchResult);
            }
        }

        public static UserDTO TestUserDTO()
        {
            var user = new User
            {
                Id = 1,
                Username = "sysadmin",
                FirstName = "Superbruker",
                LastName = "NFB",
                Email = "admin@badminton.no",
                Admin = true
            };
            return new UserDTO(user);
        }

        public static AuthResponse TestAuthResponse()
        {
            var user = new User
            {
                Id = 1,
                Username = "sysadmin",
                FirstName = "Superbruker",
                LastName = "NFB",
                Email = "admin@badminton.no",
                Admin = true
            };
            var authResponse = new AuthResponse(user)
            {
                Token = "test-token"
            };
            return authResponse;
        }

        public static NewUser TestNewUser()
        {
            var user = new NewUser
            {
                Id = 1,
                Username = "sysadmin",
                FirstName = "Superbruker",
                LastName = "NFB",
                Email = "admin@badminton.no",
                Password = "test"
            };
            return user;
        }

        public static User TestUser()
        {
            var user = new User
            {
                Id = 1,
                Username = "sysadmin",
                FirstName = "Superbruker",
                LastName = "NFB",
                Email = "admin@badminton.no",
                Admin = true
            };
            return user;
        }
    }
}
