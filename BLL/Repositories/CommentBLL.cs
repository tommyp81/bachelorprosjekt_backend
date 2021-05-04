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
                    commentDTOs.Add(new CommentDTO(comment));
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
                return new CommentDTO(getComment);
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
                return new CommentDTO (addComment);
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
                return new CommentDTO(updateComment);
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
                return new CommentDTO(deleteComment);
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
                    commentDTOs.Add(new CommentDTO(comment));
                }
                return new PageResponse<IEnumerable<CommentDTO>>(commentDTOs, comments.Count, postId, page, size, order, type);
            }
            else
            {
                return null;
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
                    commentDTOs.Add(new CommentDTO(comment));
                }
                return new PageResponse<IEnumerable<CommentDTO>>(commentDTOs, comments.Count, postId, page, size, order, type);
            }
            else
            {
                return null;
            }
        }
    }
}
