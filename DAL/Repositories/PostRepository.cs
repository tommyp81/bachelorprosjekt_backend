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

        public PostRepository(DBContext context, ICustomRepository _customRepository)
        {
            _context = context;
            this._customRepository = _customRepository;
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
            post.Date = DateTime.UtcNow.AddHours(1); // For riktig tid (UTC + 1 time)
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
            var result = await _context.Posts.FindAsync(post.Id);
            if (result != null)
            {
                result.Id = post.Id;
                result.Title = post.Title;
                result.Content = post.Content;
                result.Date = DateTime.UtcNow.AddHours(1); // For riktig tid (UTC + 1 time)
                result.UserId = post.UserId;
                result.TopicId = post.TopicId;
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

                _context.Posts.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
