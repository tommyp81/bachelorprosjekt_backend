using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICustomBLL
    {
        DocumentDTO AddDTO(Document document);
        Task<IEnumerable<DocumentDTO>> GetDocuments();
        Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId);
        Task<DocumentDTO> GetDocumentInfo(int id);
        Task<FileStreamResult> GetDocument(int id);
        Task<DocumentDTO> DeleteDocument(int id);
        Task<UserDTO> Login(string username, string email, string password);
        Task<UserDTO> SetAdmin(int id, bool admin);
    }
}
