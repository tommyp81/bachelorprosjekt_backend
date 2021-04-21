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
    public interface ICommentBLL
    {
        Task<CommentDTO> AddDTO(Comment comment);
        Task<ICollection<CommentDTO>> GetComments();
        Task<CommentDTO> GetComment(int id);
        Task<CommentDTO> AddComment(IFormFile file, Comment comment);
        Task<CommentDTO> UpdateComment(Comment comment);
        Task<CommentDTO> DeleteComment(int id);
    }
}
