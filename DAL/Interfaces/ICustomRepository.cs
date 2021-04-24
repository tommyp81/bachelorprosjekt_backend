using Microsoft.AspNetCore.Http;
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

        Task<ICollection<Document>> GetDocuments();
        Task<Document> GetDocumentInfo(int id);
        Task<Document> DeleteDocument(int id);
        Task<bool> Login(string username, string password);
    }
}
