using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        // Controller for Configuration API Backend

        private readonly ICommentBLL _commentBLL;
        private readonly IPostBLL _postBLL;
        private readonly ILikeBLL _likeBLL;

        public ConfigController(ICommentBLL commentBLL, IPostBLL postBLL, ILikeBLL likeBLL)
        {
            _commentBLL = commentBLL;
            _postBLL = postBLL;
            _likeBLL = likeBLL;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Post()
        {
            // Opprette poster med random innhold
            var posts = new List<Post>();
            //var comments = new List<Comment>();
            var random = new Random();

            // Sett instillinger her
            int postCount = 100;
            int userCount = 10;

            string[] postTitle = new string[5]
            {
                    "Dette er en testpost!",
                    "Post om et kjedelig tema...",
                    "Til den det måtte gjelde",
                    "Les dette!!!",
                    "En dag på banen"
            };

            string[] postContent = new string[5]
            {
                    "Hva syntes du om denne posten?",
                    "Det var to tomater som skulle gå over en vei.. så ja, nei!",
                    "Hei. Hvor mange likes får denne posten?",
                    "Husk å ta med gymbaggen på fredag :)",
                    "Nei forresten, bare glem det!"
            };

            // Random poster
            for (int i = 1; i <= postCount; i++)
            {
                // Instillinger for tidsone
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

                int t = random.Next(1, 5);
                int c = random.Next(1, 5);
                int userId = random.Next(2, userCount + 1);
                int subTopicId = random.Next(1, 20);

                var post = new Post
                {
                    Title = postTitle[t],
                    Content = postContent[c],
                    Date = now,
                    UserId = userId,
                    SubTopicId = subTopicId
                };

                posts.Add(post);
            }

            foreach (var post in posts)
            {
                await _postBLL.AddPost(null, post);
            }

            return Ok(posts.Count + " poster ble opprettet!");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Comment()
        {
            // Opprette poster med random innhold
            //var posts = new List<Post>();
            var comments = new List<Comment>();
            var random = new Random();

            // Sett instillinger her
            int postCount = 100;
            int commentCount = 100;
            int userCount = 10;

            string[] commentContent = new string[5]
            {
                    "Hva var det du sa fornoe?",
                    "Dette testsvaret er ikke så viktig.",
                    "Heihei, blablabla :P",
                    "Ja neida, sååå... OKEY!!!",
                    "Hei, ville bare si at jeg er helt enig"
            };

            // Random kommentarer
            for (int i = 1; i <= commentCount; i++)
            {
                // Instillinger for tidsone
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

                int c = random.Next(1, 5);
                int userId = random.Next(2, userCount + 1);
                int postId = random.Next(1, postCount);

                var comment = new Comment
                {
                    Content = commentContent[c],
                    Date = now,
                    UserId = userId,
                    PostId = postId
                };

                comments.Add(comment);
            }

            foreach (var comment in comments)
            {
                await _commentBLL.AddComment(null, comment);
            }

            return Ok(comments.Count + " kommentarer ble opprettet!");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Like()
        {
            // Opprette poster med random innhold
            var likes = new List<Like>();
            var random = new Random();

            // Sett instillinger her
            int likeCount = 100;
            int postCount = 100;
            int commentCount = 100;
            int userCount = 10;


            // Random likes
            for (int i = 1; i <= likeCount; i++)
            {
                int userId = random.Next(2, userCount + 1);
                int postId = random.Next(1, postCount);
                int commentId = random.Next(1, commentCount);

                if (postId >= commentId)
                {
                    var like = new Like
                    {
                        UserId = userId,
                        PostId = postId
                    };
                    likes.Add(like);
                }
                else
                {
                    var like = new Like
                    {
                        UserId = userId,
                        CommentId = commentId
                    };
                    likes.Add(like);
                }
            }

            foreach (var like in likes)
            {
                await _likeBLL.AddLike(like);
            }

            return Ok(likes.Count + " likes ble opprettet!");
        }
    }
}
