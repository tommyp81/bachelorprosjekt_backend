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
        private readonly IUserBLL _userBLL;

        public CustomBLL(ICustomRepository repository, ICommentRepository commentRepository, ILikeRepository likeRepository, IUserBLL userBLL)
        {
            _repository = repository;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
            _userBLL = userBLL;
        }

        // GET: GetCommentCount/1
        public async Task<int> GetCommentCount(int id)
        {
            // Telle antall kommentarer til en post på PostId
            var comments = await _commentRepository.GetComments();
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
            var likes = await _likeRepository.GetLikes();
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
            var DTO = new DocumentDTO
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
            var documents = await _repository.GetDocuments();
            if (documents == null) { return null; }
            var documentDTOs = new List<DocumentDTO>();

            foreach (var document in documents)
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
            var addDocument = await _repository.UploadDocument(file, userId, postId, commentId, infoTopicId);
            if (addDocument == null) { return null; }
            var documentDTO = AddDTO(addDocument);
            return documentDTO;
        }

        // GET: GetDocumentInfo/1
        public async Task<DocumentDTO> GetDocumentInfo(int id)
        {
            var getDocument = await _repository.GetDocumentInfo(id);
            if (getDocument == null) { return null; }
            var documentDTO = AddDTO(getDocument);
            return documentDTO;
        }

        // DELETE: DeleteDocument/1
        public async Task<DocumentDTO> DeleteDocument(int id)
        {
            var deleteDocument = await _repository.DeleteDocument(id);
            if (deleteDocument == null) { return null; }
            var documentDTO = AddDTO(deleteDocument);
            return documentDTO;
        }

        // POST: Login
        public async Task<UserDTO> Login(string username, string email, string password)
        {
            var login = await _repository.Login(username, email, password);
            if (login == null) { return null; }
            var userDTO = _userBLL.AddDTO(login);
            return userDTO;
        }

        // POST: SetAdmin
        public async Task<UserDTO> SetAdmin(int id, bool admin)
        {
            var setAdmin = await _repository.SetAdmin(id, admin);
            if (setAdmin == null) { return null; }
            var userDTO = _userBLL.AddDTO(setAdmin);
            return userDTO;
        }
    }
}
