using DAL.Helpers;
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

namespace BLL.Interfaces
{
    public interface ICustomBLL
    {
        Task<IEnumerable<DocumentDTO>> GetDocuments();
        Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId, int? infoTopicId);
        Task<DocumentDTO> GetDocumentInfo(int id);
        Task<FileStreamResult> GetDocument(int id);
        Task<DocumentDTO> DeleteDocument(int id);
        Task<AuthResponse> Login(AuthRequest request);
        Task<UserDTO> SetAdmin(int id, bool admin);
        Task<UserDTO> SetUsername(int id, string username);
        Task<PageResponse<IEnumerable<DocumentDTO>>> PagedList(int? infoTopicId, int page, int size, string order, string type);
        Task<PageResponse<IEnumerable<DocumentDTO>>> Search(string query, int? infoTopicId, int page, int size, string order, string type);
    }
}
