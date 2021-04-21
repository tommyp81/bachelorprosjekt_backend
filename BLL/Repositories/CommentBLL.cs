﻿using BLL.Interfaces;
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
        private readonly ICustomBLL _customRepository;

        public CommentBLL(ICommentRepository _repository, ICustomBLL _customRepository)
        {
            this._repository = _repository;
            this._customRepository = _customRepository;
        }

        // For å lage DTOs for Comments
        public async Task<CommentDTO> AddDTO(Comment comment)
        {
            // Telle antall likes til hver enkelt kommentar
            int likecount = await _customRepository.GetLikeCount(null, comment.Id);

            CommentDTO DTO = new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                UserId = comment.UserId,
                PostId = comment.PostId,
                DocumentId = comment.DocumentId,
                Like_Count = likecount // Like_Count vises kun med DTO
            };
            return DTO;
        }

        public async Task<ICollection<CommentDTO>> GetComments()
        {
            ICollection<Comment> comments = await _repository.GetComments();
            if (comments == null) { return null; }
            ICollection<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach (Comment comment in comments)
            {
                commentDTOs.Add(await AddDTO(comment));
            }
            return commentDTOs;
        }

        public async Task<CommentDTO> GetComment(int id)
        {
            Comment getComment = await _repository.GetComment(id);
            if (getComment == null) { return null; }
            CommentDTO commentDTO = await AddDTO(getComment);
            return commentDTO;
        }

        public async Task<CommentDTO> AddComment(IFormFile file, Comment comment)
        {
            Comment addComment = await _repository.AddComment(file, comment);
            if (addComment == null) { return null; }
            CommentDTO commentDTO = await AddDTO(addComment);
            return commentDTO;
        }

        public async Task<CommentDTO> UpdateComment(Comment comment)
        {
            Comment updateComment = await _repository.UpdateComment(comment);
            if (updateComment == null) { return null; }
            CommentDTO commentDTO = await AddDTO(updateComment);
            return commentDTO;
        }

        public async Task<CommentDTO> DeleteComment(int id)
        {
            Comment deleteComment = await _repository.DeleteComment(id);
            if (deleteComment == null) { return null; }
            CommentDTO commentDTO = await AddDTO(deleteComment);
            return commentDTO;
        }
    }
}
