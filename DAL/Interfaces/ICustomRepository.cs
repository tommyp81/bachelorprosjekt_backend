using DAL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICustomRepository
    {
        // AddDocument ligger kun her i DAL og brukes for både poster og kommentarer
        Task<Document> AddDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId);

        // UploadDocument kommer fra controller og brukes ved oppdatering av poster eller kommentarer
        Task<Document> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId);

        Task<IEnumerable<Document>> GetDocuments();
        Task<Document> GetDocumentInfo(int id);
        Task<FileStreamResult> GetDocument(int id);
        Task<Document> DeleteDocument(int id);
        Task<AuthResponse> Login(AuthRequest request);
        Task<User> SetAdmin(int id, bool admin);
        Task<Response<IEnumerable<Document>>> PagedList(int? infoTopicId, int page, int size, string order, string type);
        Task<Response<IEnumerable<Document>>> Search(string query, int? infoTopicId, int page, int size, string order, string type);
    }
}
