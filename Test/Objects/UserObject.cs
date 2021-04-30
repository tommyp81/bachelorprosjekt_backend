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
            var user = new UserDTO()
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
