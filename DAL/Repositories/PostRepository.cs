using Azure.Storage.Blobs;
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

namespace DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DBContext _context;
        private readonly ICustomRepository _customRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public PostRepository(DBContext context, ICustomRepository _customRepository, ICommentRepository _commentRepository, ILikeRepository _likeRepository)
        {
            _context = context;
            this._customRepository = _customRepository;
            this._commentRepository = _commentRepository;
            this._likeRepository = _likeRepository;
        }

        // GET: Posts
        public async Task<ICollection<Post>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        // GET: Posts/1
        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        // POST: Posts
        public async Task<Post> AddPost(IFormFile file, Post post)
        {
            // Lagre ny posten i databasen først
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            post.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
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
                    var update = await _context.Posts.FindAsync(result.Entity.Id);
                    if (update != null)
                    {
                        update.DocumentId = newDocument.Id;
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
                result.Id = post.Id;
                result.Title = post.Title;
                result.Content = post.Content;
                result.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                result.UserId = post.UserId;
                result.SubTopicId = post.SubTopicId;
                result.DocumentId = post.DocumentId;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: Posts/1
        public async Task<Post> DeletePost(int id)
        {
            var result = await _context.Posts.FindAsync(id);
            if (result != null)
            {
                // Hvis denne posten har et dokument, må dette slettes!
                if (result.DocumentId != null)
                {
                    await _customRepository.DeleteDocument((int)result.DocumentId);
                }

                // Hvis denne posten har kommentarer, må disse slettes!
                var comments = await _commentRepository.GetComments();
                foreach (var comment in comments)
                {
                    if (comment.PostId == result.Id)
                    {
                        await _commentRepository.DeleteComment(comment.Id);
                    }
                }

                // Hvis denne posten har likes, må de slettes!
                var likes = await _likeRepository.GetLikes();
                foreach (var like in likes)
                {
                    if (like.PostId == result.Id)
                    {
                        await _likeRepository.DeleteLike(like);
                    }
                }

                _context.Posts.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
