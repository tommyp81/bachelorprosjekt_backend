using DAL.Helpers;
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
        public static PageResponse<IEnumerable<CommentDTO>> TestPagedResponse(string query)
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Content = "testkommentar1",
                    Date = DateTime.UtcNow,
                    UserId = 1,
                    PostId = 1,
                    DocumentId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "testkommentar2",
                    Date = DateTime.UtcNow,
                    UserId = 2,
                    PostId = 2,
                    DocumentId = 2
                },
                new Comment
                {
                    Id = 3,
                    Content = "testkommentar3",
                    Date = DateTime.UtcNow,
                    UserId = 3,
                    PostId = 3,
                    DocumentId = 3
                },
            };

            var commentDTOs = new List<CommentDTO>();
            foreach (var comment in comments)
            {
                commentDTOs.Add(new CommentDTO(comment, "sysadmin"));
            }

            if (string.IsNullOrEmpty(query))
            {
                return new PageResponse<IEnumerable<CommentDTO>>(commentDTOs);
            }
            else
            {
                var searchResult = commentDTOs.AsQueryable().Where(q => q.Content.Contains(query));
                return new PageResponse<IEnumerable<CommentDTO>>(searchResult);
            }
        }

        public static CommentDTO TestCommentDTO()
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
            return new CommentDTO(comment, "sysadmin");
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
