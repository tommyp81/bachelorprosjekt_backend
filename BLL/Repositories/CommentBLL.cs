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
            // Telle antall likes til hver enkelt kommentar
            int likecount = await _customBLL.GetLikeCount(null, comment.Id);

            var DTO = new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                Edited = comment.Edited,
                UserId = comment.UserId,
                PostId = comment.PostId,
                DocumentId = comment.DocumentId,
                Like_Count = likecount // Like_Count vises kun med DTO
            };
            return DTO;
        }

        public async Task<ICollection<CommentDTO>> GetComments()
        {
            var comments = await _repository.GetComments();
            if (comments == null) { return null; }
            var commentDTOs = new List<CommentDTO>();
            foreach (var comment in comments)
            {
                commentDTOs.Add(await AddDTO(comment));
            }
            return commentDTOs;
        }

        public async Task<CommentDTO> GetComment(int id)
        {
            var getComment = await _repository.GetComment(id);
            if (getComment == null) { return null; }
            var commentDTO = await AddDTO(getComment);
            return commentDTO;
        }

        public async Task<CommentDTO> AddComment(IFormFile file, Comment comment)
        {
            var addComment = await _repository.AddComment(file, comment);
            if (addComment == null) { return null; }
            var commentDTO = await AddDTO(addComment);
            return commentDTO;
        }

        public async Task<CommentDTO> UpdateComment(Comment comment)
        {
            var updateComment = await _repository.UpdateComment(comment);
            if (updateComment == null) { return null; }
            var commentDTO = await AddDTO(updateComment);
            return commentDTO;
        }

        public async Task<CommentDTO> DeleteComment(int id)
        {
            var deleteComment = await _repository.DeleteComment(id);
            if (deleteComment == null) { return null; }
            var commentDTO = await AddDTO(deleteComment);
            return commentDTO;
        }
    }
}
