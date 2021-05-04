using DAL.Database_configuration;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DBContext _context;
        private readonly ICustomRepository _customRepository;

        public PostRepository(DBContext context, ICustomRepository customRepository)
        {
            _context = context;
            _customRepository = customRepository;
        }

        public async Task<int?> GetTopicId(int id)
        {
            var subTopicId = await _context.SubTopics.FindAsync(id);
            if (subTopicId != null)
            {
                return subTopicId.TopicId;
            }
            else
            {
                return null;
            }
        }

        // GET: Posts
        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            if (posts != null)
            {
                return posts;
            }
            else
            {
                return null;
            }
        }

        // GET: Posts/1
        public async Task<Post> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                return post;
            }
            else
            {
                return null;
            }
        }

        // POST: Posts
        public async Task<Post> AddPost(IFormFile file, Post post)
        {
            // Lagre ny posten i databasen først
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            post.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            post.Edited = false;
            post.Comment_Count = 0;
            post.Like_Count = 0;

            var result = await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Se etter fil og last opp hvis den er sendt med
            if (file != null)
            {
                if (file.Length > 0)
                {
                    // Kaller på AddDocument metoden fra CustomRepository, så vi får en ny oppføring i databasen til Documents
                    var newDocument = await _customRepository.AddDocument(file, result.Entity.UserId, result.Entity.Id, null, null);

                    // Oppdater denne posten med DocumentId
                    var updatePost = await _context.Posts.FindAsync(result.Entity.Id);
                    if (updatePost != null)
                    {
                        updatePost.DocumentId = newDocument.Id;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return result.Entity;
        }

        // PUT: Posts/1
        public async Task<Post> UpdatePost(Post post)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var result = await _context.Posts.FindAsync(post.Id);
            if (result != null)
            {
                result.Id = result.Id;
                result.Title = post.Title;
                result.Content = post.Content;
                result.Date = result.Date;
                result.EditDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                result.Edited = true;
                result.Comment_Count = result.Comment_Count;
                result.Like_Count = result.Like_Count;
                result.UserId = result.UserId;
                result.SubTopicId = result.SubTopicId;
                result.DocumentId = result.DocumentId;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        // DELETE: Posts/1
        public async Task<Post> DeletePost(int id)
        {
            var result = await _context.Posts.FindAsync(id);
            if (result != null)
            {
                // Hvis denne posten har likes, må de slettes!
                var likes = await _context.Likes.ToListAsync();
                likes.RemoveAll(l => l.PostId == result.Id);

                // Hvis denne posten hører til en video, må denne slettes!
                var videos = await _context.Videos.ToListAsync();
                // Slettes direkte fra databasen siden video også bruker denne metoden
                videos.RemoveAll(v => v.PostId == result.Id);

                // Hvis denne posten har et dokument, må dette slettes!
                if (result.DocumentId != null)
                {
                    await _customRepository.DeleteDocument((int)result.DocumentId);
                }

                // Hvis denne posten har kommentarer, må disse slettes!
                var comments = _context.Comments.ToList().Where(c => c.PostId == result.Id);
                if (comments != null)
                {
                    foreach (var comment in comments)
                    {
                        // Hvis denne kommentaren har likes, må disse fjærnes
                        likes.RemoveAll(l => l.CommentId == comment.Id);

                        // Hvis denne kommentaren har et dokument, må dette fjærnes
                        if (comment.DocumentId != null)
                        {
                            await _customRepository.DeleteDocument((int)comment.DocumentId);
                        }

                        // Fjærn kommentaren
                        _context.Comments.Remove(comment);
                    }
                }

                // Fjærn posten og lagre alle endringer til databasen
                _context.Posts.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Response<IEnumerable<Post>>> PagedList(int? subTopicId, int page, int size, string order, string type)
        {
            IEnumerable<Post> list;
            if (subTopicId != null)
            {
                list = await _context.Posts.AsQueryable().Where(q => q.SubTopicId == subTopicId).OrderBy(type + " " + order).ToListAsync();
            }
            else
            {
                list = await _context.Posts.AsQueryable().OrderBy(type + " " + order).ToListAsync();
            }

            if (list != null)
            {
                var count = list.Count();
                if (count != 0)
                {
                    var pagedList = await list.ToPagedListAsync(page, size);
                    return new Response<IEnumerable<Post>>(pagedList, count);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Response<IEnumerable<Post>>> Search(string query, int? subTopicId, int page, int size, string order, string type)
        {
            if (!string.IsNullOrEmpty(query))
            {
                IEnumerable<Post> list;
                if (subTopicId != null)
                {
                    list = await _context.Posts.AsQueryable().Where(q => q.SubTopicId == subTopicId).OrderBy(type + " " + order).ToListAsync();
                }
                else
                {
                    list = await _context.Posts.AsQueryable().OrderBy(type + " " + order).ToListAsync();
                }

                var searchList = list.Where(q => q.Title.Contains(query) || q.Content.Contains(query));
                var count = searchList.Count();
                if (count != 0)
                {
                    var pagedSearchList = await searchList.ToPagedListAsync(page, size);
                    return new Response<IEnumerable<Post>>(pagedSearchList, count);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
