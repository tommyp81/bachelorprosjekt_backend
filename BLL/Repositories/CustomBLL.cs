using BLL.Interfaces;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
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

        public CustomBLL(ICustomRepository repository)
        {
            _repository = repository;
        }

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
                        documentDTOs.Add(new DocumentDTO(document));
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

        public async Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId)
        {
            var addDocument = await _repository.UploadDocument(file, userId, postId, commentId, infoTopicId);
            if (addDocument != null)
            {
                return new DocumentDTO(addDocument);
            }
            else
            {
                return null;
            }
        }

        public async Task<DocumentDTO> GetDocumentInfo(int id)
        {
            var getDocument = await _repository.GetDocumentInfo(id);
            if (getDocument != null)
            {
                return new DocumentDTO(getDocument);
            }
            else
            {
                return null;
            }
        }

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

        public async Task<DocumentDTO> DeleteDocument(int id)
        {
            var deleteDocument = await _repository.DeleteDocument(id);
            if (deleteDocument != null)
            {
                return new DocumentDTO(deleteDocument);
            }
            else
            {
                return null;
            }
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var response = await _repository.Login(request);
            if (response != null)
            {
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> SetAdmin(int id, bool admin)
        {
            var setAdmin = await _repository.SetAdmin(id, admin);
            if (setAdmin != null)
            {
                return new UserDTO(setAdmin);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> SetUsername(int id, string username)
        {
            var setAdmin = await _repository.SetUsername(id, username);
            if (setAdmin != null)
            {
                return new UserDTO(setAdmin);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<DocumentDTO>>> PagedList(int? infoTopicId, int page, int size, string order, string type)
        {
            var documents = await _repository.PagedList(infoTopicId, page, size, order, type);
            if (documents != null)
            {
                var documentDTOs = new List<DocumentDTO>();
                foreach (var document in documents.Data)
                {
                    documentDTOs.Add(new DocumentDTO(document));
                }
                return new PageResponse<IEnumerable<DocumentDTO>>(documentDTOs, documents.Count, infoTopicId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<DocumentDTO>>(null, 0, infoTopicId, page, size, order, type);
            }
        }

        public async Task<PageResponse<IEnumerable<DocumentDTO>>> Search(string query, int? infoTopicId, int page, int size, string order, string type)
        {
            var documents = await _repository.Search(query, infoTopicId, page, size, order, type);
            if (documents != null)
            {
                var documentDTOs = new List<DocumentDTO>();
                foreach (var document in documents.Data)
                {
                    documentDTOs.Add(new DocumentDTO(document));
                }
                return new PageResponse<IEnumerable<DocumentDTO>>(documentDTOs, documents.Count, infoTopicId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<DocumentDTO>>(null, 0, infoTopicId, page, size, order, type);
            }
        }
    }
}
