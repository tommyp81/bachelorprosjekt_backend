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
    }
}
