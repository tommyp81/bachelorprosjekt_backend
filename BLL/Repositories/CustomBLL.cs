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
    public class CustomBLL : ICustomBLL
    {
        private readonly ICustomRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public CustomBLL(ICustomRepository _repository, ICommentRepository _commentRepository, ILikeRepository _likeRepository)
        {
            this._repository = _repository;
            this._commentRepository = _commentRepository;
            this._likeRepository = _likeRepository;
        }

        // GET: GetCommentCount/1
        public async Task<int> GetCommentCount(int id)
        {
            // Telle antall kommentarer til en post på PostId
            ICollection<Comment> comments = await _commentRepository.GetComments();
            if (comments == null) { return 0; }
            var commentcount = from comment in comments.AsEnumerable()
                               where comment.PostId == id
                               select comment;

            return commentcount.Count();
        }

        // GET: GetLikeCount
        public async Task<int> GetLikeCount(int? postId, int? commentId)
        {
            // Telle antall likes til poster eller kommentarer
            ICollection<Like> likes = await _likeRepository.GetLikes();
            if (likes == null) { return 0; }

            // Telle for poster hvis vi får postId
            if (postId != null)
            {
                var likecount = from like in likes.AsEnumerable()
                                where like.PostId == postId
                                select like;

                return likecount.Count();
            }

            // Telle for kommentarer hvis vi får commentId
            if (commentId != null)
            {
                var likecount = from like in likes.AsEnumerable()
                                where like.CommentId == commentId
                                select like;

                return likecount.Count();
            }

            return 0;
        }

        // For å lage DTOs for Documents
        public DocumentDTO AddDTO(Document document)
        {
            DocumentDTO DTO = new DocumentDTO
            {
                Id = document.Id,
                FileName = document.FileName,
                FileType = document.FileType,
                FileSize = document.FileSize,
                Uploaded = document.Uploaded,
                UniqueName = document.UniqueName,
                Container = document.Container,
                UserId = document.UserId,
                PostId = document.PostId,
                CommentId = document.CommentId,
                InfoTopicId = document.InfoTopicId
            };
            return DTO;
        }

        // GET: GetDocuments
        public async Task<ICollection<DocumentDTO>> GetDocuments()
        {
            ICollection<Document> documents = await _repository.GetDocuments();
            if (documents == null) { return null; }
            ICollection<DocumentDTO> documentDTOs = new List<DocumentDTO>();

            foreach (Document document in documents)
            {
                // Sjekke om dokumentet har InfoTopicId
                if (document.InfoTopicId != null)
                {
                    // Legg dokumentet til i listen
                    documentDTOs.Add(AddDTO(document));
                }
            }

            // Returnerer alle dokumenter som har en InfoTopicId
            return documentDTOs;
        }

        // POST: AddDocument
        public async Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId)
        {
            Document addDocument = await _repository.UploadDocument(file, userId, postId, commentId, infoTopicId);
            if (addDocument == null) { return null; }
            DocumentDTO documentDTO = AddDTO(addDocument);
            return documentDTO;
        }

        // GET: GetDocumentInfo/1
        public async Task<DocumentDTO> GetDocumentInfo(int id)
        {
            Document getDocument = await _repository.GetDocumentInfo(id);
            if (getDocument == null) { return null; }
            DocumentDTO documentDTO = AddDTO(getDocument);
            return documentDTO;
        }

        // DELETE: DeleteDocument/1
        public async Task<DocumentDTO> DeleteDocument(int id)
        {
            Document deleteDocument = await _repository.DeleteDocument(id);
            if (deleteDocument == null) { return null; }
            DocumentDTO documentDTO = AddDTO(deleteDocument);
            return documentDTO;
        }

        // POST: Login
        public async Task<UserDTO> Login(string username, string password)
        {
            var user = await _repository.Login(username, password);
            if (user != null)
            {
                // Login ok, return userDTO
                UserDTO userDTO = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    isAdmin = user.isAdmin
                };
                return userDTO;
            }
            return null;
        }
    }
}
