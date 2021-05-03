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

        public CommentBLL(ICommentRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Comments
        public CommentDTO AddDTO(Comment comment)
        {
            var DTO = new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                EditDate = comment.EditDate,
                Edited = comment.Edited,
                Like_Count = comment.Like_Count,
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
                    commentDTOs.Add(AddDTO(comment));
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
                return AddDTO(getComment);
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
                return AddDTO(addComment);
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
                return AddDTO(updateComment);
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
                return AddDTO(deleteComment);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<CommentDTO>> PagedList(int? postId, int page, int size, string order, string type)
        {
            var comments = await _repository.PagedList(postId, page, size, order, type);
            if (comments != null)
            {
                var commentDTOs = new List<CommentDTO>();
                foreach (var comment in comments)
                {
                    commentDTOs.Add(AddDTO(comment));
                }
                return commentDTOs;
            }
            else
            {
                return null;
            }
        }
    }
}
