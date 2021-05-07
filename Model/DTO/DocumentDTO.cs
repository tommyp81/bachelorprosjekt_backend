using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public DateTime Uploaded { get; set; }
        public string UniqueName { get; set; }
        public string Container { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? InfoTopicId { get; set; }

        public DocumentDTO(Document document)
        {
            Id = document.Id;
            FileName = document.FileName;
            FileType = document.FileType;
            FileSize = document.FileSize;
            Uploaded = document.Uploaded;
            UniqueName = document.UniqueName;
            Container = document.Container;
            UserId = document.UserId;
            PostId = document.PostId;
            CommentId = document.CommentId;
            InfoTopicId = document.InfoTopicId;
        }
    }
}
