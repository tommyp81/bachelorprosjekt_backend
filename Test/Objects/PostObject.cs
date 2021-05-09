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
    public class PostObject
    {
        public static PageResponse<IEnumerable<PostDTO>> TestPagedResponse(string query)
        {
            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Test1",
                    Content = "testpost1",
                    Date = DateTime.UtcNow,
                    UserId = 1,
                    SubTopicId = 1
                },
                new Post
                {
                    Id = 2,
                    Title = "Test2",
                    Content = "testpost2",
                    Date = DateTime.UtcNow,
                    UserId = 2,
                    SubTopicId = 2
                },
                new Post
                {
                    Id = 3,
                    Title = "Test3",
                    Content = "testpost3",
                    Date = DateTime.UtcNow,
                    UserId = 3,
                    SubTopicId = 3
                },
            };

            var postDTOs = new List<PostDTO>();
            foreach (var post in posts)
            {
                postDTOs.Add(new PostDTO(post, "sysadmin", null));
            }

            if (string.IsNullOrEmpty(query))
            {
                return new PageResponse<IEnumerable<PostDTO>>(postDTOs);
            }
            else
            {
                var searchResult = postDTOs.AsQueryable().Where(q => q.Content.Contains(query));
                return new PageResponse<IEnumerable<PostDTO>>(searchResult);
            }
        }

        public static PostDTO TestPostDTO()
        {
            var post = new Post()
            {
                Id = 1,
                Title = "Test1",
                Content = "testpost1",
                Date = DateTime.UtcNow,
                UserId = 1,
                SubTopicId = 1
            };
            return new PostDTO(post, "sysadmin", null);
        }

        public static Post TestPost()
        {
            var post = new Post()
            {
                Id = 1,
                Title = "Test1",
                Content = "testpost1",
                Date = DateTime.UtcNow,
                UserId = 1,
                SubTopicId = 1,
            };
            return post;
        }
    }
}
