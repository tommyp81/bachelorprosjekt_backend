using DAL.Helpers;
using Model.Domain_models;
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
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUser(int id);
        Task<UserDTO> AddUser(NewUser user);
        Task<UserDTO> UpdateUser(NewUser user);
        Task<UserDTO> DeleteUser(int id);
        Task<PageResponse<IEnumerable<UserDTO>>> PagedList(int page, int size, string order, string type);
        Task<PageResponse<IEnumerable<UserDTO>>> Search(string query, int page, int size, string order, string type);
    }
}
