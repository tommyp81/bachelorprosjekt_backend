using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<Post> GetPost(int id);
        Task<Post> AddPost(IFormFile file, Post post);
        Task<Post> UpdatePost(Post post);
        Task<Post> DeletePost(int id);
        Task<IEnumerable<Post>> PostPaging(int? page, int? count, string order, string type);
    }
}
