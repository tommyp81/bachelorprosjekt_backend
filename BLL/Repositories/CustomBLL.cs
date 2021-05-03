using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserBLL _userBLL;

        public CustomBLL(ICustomRepository repository, IUserBLL userBLL)
        {
            _repository = repository;
            _userBLL = userBLL;
        }

        //// GET: GetCommentCount
        //public async Task<int> Comment_Count(int postId)
        //{
        //    // Telle antall kommentarer til en post på PostId
        //    var comments = await _commentRepository.GetComments(postId);
        //    if (comments != null)
        //    {
        //        return comments.Count();
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //// GET: GetLikeCount
        //public async Task<int> Like_Count(int? postId, int? commentId)
        //{
        //    // Telle antall likes til poster eller kommentarer
        //    var likes = await _likeRepository.GetLikes();
        //    if (likes != null)
        //    {
        //        // Telle for poster hvis vi får postId
        //        if (postId != null)
        //        {
        //            var likeCount = likes.Where(l => l.PostId == postId).AsEnumerable();
        //            if (likeCount != null)
        //            {
        //                return likeCount.Count();
        //            }
        //        }

        //        // Telle for kommentarer hvis vi får commentId
        //        if (commentId != null)
        //        {
        //            var likeCount = likes.Where(l => l.CommentId == commentId).AsEnumerable();
        //            if (likeCount != null)
        //            {
        //                return likeCount.Count();
        //            }
        //        }
        //    }

        //    return 0;
        //}

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
        public async Task<IEnumerable<DocumentDTO>> GetDocuments()
        {
            var documents = await _repository.GetDocuments();
            if (documents != null)
            {
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
            else
            {
                return null;
            }
        }

        // POST: AddDocument
        public async Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId)
        {
            var addDocument = await _repository.UploadDocument(file, userId, postId, commentId, infoTopicId);
            if (addDocument != null)
            {
                return AddDTO(addDocument);
            }
            else
            {
                return null;
            }
        }

        // GET: GetDocumentInfo/1
        public async Task<DocumentDTO> GetDocumentInfo(int id)
        {
            var getDocument = await _repository.GetDocumentInfo(id);
            if (getDocument != null)
            {
                return AddDTO(getDocument);
            }
            else
            {
                return null;
            }
        }

        // GET: GetDocument/1
        public async Task<FileStreamResult> GetDocument(int id)
        {
            var file = await _repository.GetDocument(id);
            if (file != null)
            {
                return file;
            }
            else
            {
                return null;
            }
        }

        // DELETE: DeleteDocument/1
        public async Task<DocumentDTO> DeleteDocument(int id)
        {
            var deleteDocument = await _repository.DeleteDocument(id);
            if (deleteDocument != null)
            {
                return AddDTO(deleteDocument);
            }
            else
            {
                return null;
            }
        }

        // POST: Login
        public async Task<UserDTO> Login(string username, string email, string password)
        {
            var login = await _repository.Login(username, email, password);
            if (login != null)
            {
                return _userBLL.AddDTO(login);
            }
            else
            {
                return null;
            }
        }

        // POST: SetAdmin
        public async Task<UserDTO> SetAdmin(int id, bool admin)
        {
            var setAdmin = await _repository.SetAdmin(id, admin);
            if (setAdmin != null)
            {
                return _userBLL.AddDTO(setAdmin);
            }
            else
            {
                return null;
            }
        }

        // GET: GetCount
        public async Task<int> GetCount(string type, int? id)
        {
            return await _repository.GetCount(type, id);
        }

        public async Task<IEnumerable<DocumentDTO>> PagedList(int? infoTopicId, int page, int size, string order, string type)
        {
            var documents = await _repository.PagedList(infoTopicId, page, size, order, type);
            if (documents != null)
            {
                var documentDTOs = new List<DocumentDTO>();
                foreach (var video in documents)
                {
                    documentDTOs.Add(AddDTO(video));
                }
                return documentDTOs;
            }
            else
            {
                return null;
            }
        }
    }
}
