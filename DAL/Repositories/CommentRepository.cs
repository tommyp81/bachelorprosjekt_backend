using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DBContext _context;
        private readonly ICustomRepository _customRepository;
        private readonly ILikeRepository _likeRepository;

        public CommentRepository(DBContext context, ICustomRepository customRepository, ILikeRepository likeRepository)
        {
            _context = context;
            _customRepository = customRepository;
            _likeRepository = likeRepository;
        }

        // GET: Comment
        public async Task<ICollection<Comment>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: Comment/1
        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        // POST: Comment
        public async Task<Comment> AddComment(IFormFile file, Comment comment)
        {
            // Lagre ny kommentar i databasen først
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            comment.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            comment.Edited = false;
            var result = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Se etter fil og last opp hvis den er sendt med
            if (file != null)
            {
                if (file.Length > 0)
                {
                    // Kaller på AddDocument metoden fra CustomRepository, så vi får en ny oppføring i databasen til Documents
                    var newDocument = await _customRepository.AddDocument(file, result.Entity.UserId, null, result.Entity.Id, null);

                    // Oppdater denne kommentaren med DocumentId
                    var update = await _context.Comments.FindAsync(result.Entity.Id);
                    if (update != null)
                    {
                        update.DocumentId = newDocument.Id;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return result.Entity;
        }

        // PUT: Comment/1
        public async Task<Comment> UpdateComment(Comment comment)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var result = await _context.Comments.FindAsync(comment.Id);
            if (result != null)
            {
                result.Id = comment.Id;
                result.Content = comment.Content;
                result.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                result.Edited = true;
                result.UserId = comment.UserId;
                result.PostId = comment.PostId;
                result.DocumentId = comment.DocumentId;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: Comment/1
        public async Task<Comment> DeleteComment(int id)
        {
            var result = await _context.Comments.FindAsync(id);
            if (result != null)
            {
                // Hvis denne kommentaren har et dokument, må dette slettes!
                if (result.DocumentId != null)
                {
                    await _customRepository.DeleteDocument((int)result.DocumentId);
                }

                // Hvis denne kommentaren har likes, må de slettes!
                var likes = await _likeRepository.GetLikes();
                foreach (var like in likes)
                {
                    if (like.CommentId == result.Id)
                    {
                        await _likeRepository.DeleteLike(like);
                    }
                }

                _context.Comments.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
