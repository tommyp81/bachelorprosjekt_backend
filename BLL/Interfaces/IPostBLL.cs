using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPostBLL
    {
        Task<PostDTO> AddDTO(Post post);
        Task<IEnumerable<PostDTO>> GetPosts();
        Task<PostDTO> GetPost(int id);
        Task<PostDTO> AddPost(IFormFile file, Post post);
        Task<PostDTO> UpdatePost(Post post);
        Task<PostDTO> DeletePost(int id);
        Task<IEnumerable<PostDTO>> PostPaging(int? page, int? count, string order, string type);
    }
}
