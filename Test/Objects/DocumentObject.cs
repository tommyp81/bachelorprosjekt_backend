using Microsoft.AspNetCore.Mvc;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    class DocumentObject
    {
        public static ICollection<DocumentDTO> TestDocumentListDTO()
        {
            var documents = new List<DocumentDTO>
            {
                new DocumentDTO(null)
                {
                    Id = 1,
                    FileName = "Testfil1.txt",
                    FileType = ".txt",
                    FileSize = "17 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "(test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = 1,
                    CommentId = null,
                    InfoTopicId = null
                },
                new DocumentDTO(null)
                {
                    Id = 2,
                    FileName = "Testfil2.txt",
                    FileType = ".txt",
                    FileSize = "17 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "(test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = null,
                    CommentId = 1,
                    InfoTopicId = null
                },
                new DocumentDTO(null)
                {
                    Id = 3,
                    FileName = "Testfil3.txt",
                    FileType = ".txt",
                    FileSize = "17 byte",
                    Uploaded = DateTime.UtcNow,
                    UniqueName = "(test)",
                    Container = "sysadmin",
                    UserId = 1,
                    PostId = null,
                    CommentId = null,
                    InfoTopicId = 1
                },
            };
            return documents;
        }

        public static DocumentDTO TestDocumentDTO()
        {
            var document = new DocumentDTO(null)
            {
                Id = 1,
                FileName = "Testfil1.txt",
                FileType = ".txt",
                FileSize = "17 byte",
                Uploaded = DateTime.UtcNow,
                UniqueName = "(test)",
                Container = "sysadmin",
                UserId = 1,
                PostId = 1,
                CommentId = null,
                InfoTopicId = null
            };
            return document;
        }

        public static Document TestDocument()
        {
            var document = new Document()
            {
                Id = 1,
                FileName = "Testfil1.txt",
                FileType = ".txt",
                FileSize = "17 byte",
                Uploaded = DateTime.UtcNow,
                UniqueName = "(test)",
                Container = "sysadmin",
                UserId = 1,
                PostId = 1,
                CommentId = null,
                InfoTopicId = null
            };
            return document;
        }

        public static FileStreamResult TestFile()
        {
            var stream = Encoding.ASCII.GetBytes("Dette er en test.");
            var content = new MemoryStream(stream);
            var contentType = "text/plain";
            var fileName = "Testfil1.txt";
            return new FileStreamResult(content, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
