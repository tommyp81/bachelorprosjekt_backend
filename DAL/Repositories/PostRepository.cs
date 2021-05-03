using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<IEnumerable<Post>> PagedList(int? subTopicId, int page, int size, string order, string type)
        {
            if (subTopicId != null)
            {
                var pagedList = await _context.Posts.Where(p => p.SubTopicId == subTopicId).ToListAsync();
                return order switch
                {
                    "Desc" => type switch
                    {
                        "Date" => await pagedList.OrderByDescending(p => p.Date).ToPagedListAsync(page, size),
                        "Like" => await pagedList.OrderByDescending(p => p.Like_Count).ToPagedListAsync(page, size),
                        "Comment" => await pagedList.OrderByDescending(p => p.Comment_Count).ToPagedListAsync(page, size),
                        _ => await pagedList.OrderByDescending(p => p.Id).ToPagedListAsync(page, size),
                    },
                    "Asc" => type switch
                    {
                        "Date" => await pagedList.OrderBy(p => p.Date).ToPagedListAsync(page, size),
                        "Like" => await pagedList.OrderBy(p => p.Like_Count).ToPagedListAsync(page, size),
                        "Comment" => await pagedList.OrderBy(p => p.Comment_Count).ToPagedListAsync(page, size),
                        _ => await pagedList.OrderBy(p => p.Id).ToPagedListAsync(page, size),
                    },
                    _ => await pagedList.OrderBy(p => p.Date).ToPagedListAsync(page, size),
                };
            }
            else
            {
                var pagedList = await _context.Posts.ToListAsync();
                return order switch
                {
                    "Desc" => type switch
                    {
                        "Date" => await pagedList.OrderByDescending(p => p.Date).ToPagedListAsync(page, size),
                        "Like" => await pagedList.OrderByDescending(p => p.Like_Count).ToPagedListAsync(page, size),
                        "Comment" => await pagedList.OrderByDescending(p => p.Comment_Count).ToPagedListAsync(page, size),
                        _ => await pagedList.OrderByDescending(p => p.Id).ToPagedListAsync(page, size),
                    },
                    "Asc" => type switch
                    {
                        "Date" => await pagedList.OrderBy(p => p.Date).ToPagedListAsync(page, size),
                        "Like" => await pagedList.OrderBy(p => p.Like_Count).ToPagedListAsync(page, size),
                        "Comment" => await pagedList.OrderBy(p => p.Comment_Count).ToPagedListAsync(page, size),
                        _ => await pagedList.OrderBy(p => p.Id).ToPagedListAsync(page, size),
                    },
                    _ => await pagedList.OrderBy(p => p.Date).ToPagedListAsync(page, size),
                };
            }
        }
    }
}
