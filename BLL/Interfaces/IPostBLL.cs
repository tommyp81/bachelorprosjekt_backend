using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using Model.DTO;
using Model.Wrappers;
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
        Task<PageResponse<IEnumerable<PostDTO>>> PagedList(int? subTopicId, int page, int size, string order, string type);
        Task<PageResponse<IEnumerable<PostDTO>>> Search(string query, int? subTopicId, int page, int size, string order, string type);
    }
}
