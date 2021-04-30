using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class CommentObject
    {
        public static ICollection<CommentDTO> TestCommentListDTO()
        {
            var comments = new List<CommentDTO>
            {
                new CommentDTO()
                {
                    Id = 1,
                    Content = "testkommentar1",
                    Date = DateTime.UtcNow,
                    UserId = 1,
                    PostId = 1,
                    DocumentId = 1
                },
                new CommentDTO()
                {
                    Id = 2,
                    Content = "testkommentar2",
                    Date = DateTime.UtcNow,
                    UserId = 2,
                    PostId = 2,
                    DocumentId = 2
                },
                new CommentDTO()
                {
                    Id = 3,
                    Content = "testkommentar3",
                    Date = DateTime.UtcNow,
                    UserId = 3,
                    PostId = 3,
                    DocumentId = 3
                },
            };
            return comments;
        }

        public static CommentDTO TestCommentDTO()
        {
            var comment = new CommentDTO()
            {
                Id = 1,
                Content = "testkommentar1",
                Date = DateTime.UtcNow,
                UserId = 1,
                PostId = 1,
                DocumentId = 1
            };
            return comment;
        }

        public static Comment TestComment()
        {
            var comment = new Comment()
            {
                Id = 1,
                Content = "testkommentar1",
                Date = DateTime.UtcNow,
                UserId = 1,
                PostId = 1,
                DocumentId = 1
            };
            return comment;
        }
    }
}
