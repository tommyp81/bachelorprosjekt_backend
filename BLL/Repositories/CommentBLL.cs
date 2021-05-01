using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class CommentBLL : ICommentBLL
    {
        private readonly ICommentRepository _repository;
        private readonly ICustomBLL _customBLL;

        public CommentBLL(ICommentRepository repository, ICustomBLL customBLL)
        {
            _repository = repository;
            _customBLL = customBLL;
        }

        // For å lage DTOs for Comments
        public async Task<CommentDTO> AddDTO(Comment comment)
        {
            // Antall likes til kommentarer
            var likeCount = await _customBLL.Like_Count(null, comment.Id);

            var DTO = new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                EditDate = comment.EditDate,
                Edited = comment.Edited,
                Like_Count = likeCount, // Kun her som DTO
                UserId = comment.UserId,
                PostId = comment.PostId,
                DocumentId = comment.DocumentId
            };

            return DTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetComments(int? postId)
        {
            var getComments = await _repository.GetComments(postId);
            if (getComments != null)
            {
                var commentDTOs = new List<CommentDTO>();
                foreach (var comment in getComments)
                {
                    commentDTOs.Add(await AddDTO(comment));
                }
                return commentDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<CommentDTO> GetComment(int id)
        {
            var getComment = await _repository.GetComment(id);
            if (getComment != null)
            {
                return await AddDTO(getComment);
            }
            else
            {
                return null;
            }
        }

        public async Task<CommentDTO> AddComment(IFormFile file, Comment comment)
        {
            var addComment = await _repository.AddComment(file, comment);
            if (addComment != null)
            {
                return await AddDTO(addComment);
            }
            else
            {
                return null;
            }
        }

        public async Task<CommentDTO> UpdateComment(Comment comment)
        {
            var updateComment = await _repository.UpdateComment(comment);
            if (updateComment != null)
            {
                return await AddDTO(updateComment);
            }
            else
            {
                return null;
            }
        }

        public async Task<CommentDTO> DeleteComment(int id)
        {
            var deleteComment = await _repository.DeleteComment(id);
            if (deleteComment != null)
            {
                return await AddDTO(deleteComment);
            }
            else
            {
                return null;
            }
        }
    }
}
