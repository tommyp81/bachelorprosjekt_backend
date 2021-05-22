using BLL.Interfaces;
using DAL.Helpers;
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

        public async Task<IEnumerable<CommentDTO>> GetComments(int? postId)
        {
            var getComments = await _repository.GetComments(postId);
            if (getComments != null)
            {
                var commentDTOs = new List<CommentDTO>();
                foreach (var comment in getComments)
                {
                    var username = await _repository.GetUsername(comment.UserId);
                    commentDTOs.Add(new CommentDTO(comment, username));
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
                var username = await _repository.GetUsername(getComment.UserId);
                return new CommentDTO(getComment, username);
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
                var username = await _repository.GetUsername(addComment.UserId);
                return new CommentDTO (addComment, username);
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
                var username = await _repository.GetUsername(updateComment.UserId);
                return new CommentDTO(updateComment, username);
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
                var username = await _repository.GetUsername(deleteComment.UserId);
                return new CommentDTO(deleteComment, username);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<CommentDTO>>> PagedList(int? postId, int page, int size, string order, string type)
        {
            var comments = await _repository.PagedList(postId, page, size, order, type);
            if (comments != null)
            {
                var commentDTOs = new List<CommentDTO>();
                foreach (var comment in comments.Data)
                {
                    var username = await _repository.GetUsername(comment.UserId);
                    commentDTOs.Add(new CommentDTO(comment, username));
                }
                return new PageResponse<IEnumerable<CommentDTO>>(commentDTOs, comments.Count, postId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<CommentDTO>>(null, 0, postId, page, size, order, type);
            }
        }

        public async Task<PageResponse<IEnumerable<CommentDTO>>> Search(string query, int? postId, int page, int size, string order, string type)
        {
            var comments = await _repository.Search(query, postId, page, size, order, type);
            if (comments != null)
            {
                var commentDTOs = new List<CommentDTO>();
                foreach (var comment in comments.Data)
                {
                    var username = await _repository.GetUsername(comment.UserId);
                    commentDTOs.Add(new CommentDTO(comment, username));
                }
                return new PageResponse<IEnumerable<CommentDTO>>(commentDTOs, comments.Count, postId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<CommentDTO>>(null, 0, postId, page, size, order, type);
            }
        }
    }
}
