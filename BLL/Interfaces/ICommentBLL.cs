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
    public interface ICommentBLL
    {
        CommentDTO AddDTO(Comment comment);
        Task<IEnumerable<CommentDTO>> GetComments(int? postId);
        Task<CommentDTO> GetComment(int id);
        Task<CommentDTO> AddComment(IFormFile file, Comment comment);
        Task<CommentDTO> UpdateComment(Comment comment);
        Task<CommentDTO> DeleteComment(int id);
        Task<PageResponse<IEnumerable<CommentDTO>>> PagedList(int? postId, int page, int size, string order, string type);
        Task<PageResponse<IEnumerable<CommentDTO>>> Search(string query, int? postId, int page, int size, string order, string type);
    }
}
