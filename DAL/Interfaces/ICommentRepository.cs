using DAL.Helpers;
using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetComments(int? postId);
        Task<Comment> GetComment(int id);
        Task<Comment> AddComment(IFormFile file, Comment comment);
        Task<Comment> UpdateComment(Comment comment);
        Task<Comment> DeleteComment(int id);
        Task<Response<IEnumerable<Comment>>> PagedList(int? postId, int page, int size, string order, string type);
        Task<Response<IEnumerable<Comment>>> Search(string query, int? postId, int page, int size, string order, string type);
    }
}
