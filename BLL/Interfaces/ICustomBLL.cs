using Microsoft.AspNetCore.Http;
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
        // GetCommentCount ligger kun i BLL, og brukes bare til DTOs
        Task<int> GetCommentCount(int id);

        // GetLikeCount ligger kun i BLL, og brukes bare til DTOs
        Task<int> GetLikeCount(int? postId, int? commentId);

        DocumentDTO AddDTO(Document document);
        Task<DocumentDTO> UploadDocument(IFormFile file, int? userId, int? postId, int? commentId);
        Task<DocumentDTO> GetDocumentInfo(int id);
        Task<DocumentDTO> DeleteDocument(int id);
    }
}
