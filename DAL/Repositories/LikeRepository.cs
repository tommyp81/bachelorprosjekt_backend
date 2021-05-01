using DAL.Database_configuration;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DBContext _context;

        public LikeRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetLikes()
        {
            var likes = await _context.Likes.ToListAsync();
            if (likes != null)
            {
                return likes;
            }
            else
            {
                return null;
            }
        }

        // POST: GetLike
        public async Task<Like> GetLike(Like like)
        {
            // GetLike skal finne likes med UserId og PostId eller CommentId
            var allLikes = await _context.Likes.ToListAsync();
            var likesByUser = allLikes.Where(l => l.UserId == like.UserId);

            if (like.PostId != null)
            {
                // Finn riktig like med PostId hvis denne er lagt ved
                var postLike = likesByUser.FirstOrDefault(l => l.PostId == like.PostId);
                if (postLike != null)
                {
                    return postLike;
                }
                else
                {
                    return null;
                }
            }
            else if (like.CommentId != null)
            {
                // Finn riktig like med CommentId hvis denne er lagt ved
                var commentLike = likesByUser.FirstOrDefault(l => l.CommentId == like.CommentId);
                if (commentLike != null)
                {
                    return commentLike;
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

        // POST: AddLike
        public async Task<Like> AddLike(Like like)
        {
            // Sjekk om denne eksisterer
            var updateLike = await GetLike(like);

            // Oppdater om den finnes
            if (updateLike != null)
            {
                //var updateLike = await _context.Likes.FindAsync(getLike.Id);
                //updateLike.Id = updateLike.Id;
                //updateLike.UserId = updateLike.UserId;

                if (updateLike.PostId != null)
                {
                    // Oppdater post om det er en postId
                    var post = await _context.Posts.FindAsync(updateLike.PostId);
                    if (post != null)
                    {
                        post.Like_Count++;
                        updateLike.PostId = like.PostId;
                    }
                }
                else if (updateLike.CommentId != null)
                {
                    // Oppdater kommentar om det er en commentId
                    var comment = await _context.Comments.FindAsync(updateLike.CommentId);
                    if (comment != null)
                    {
                        comment.Like_Count++;
                        updateLike.CommentId = like.CommentId;
                    }
                }

                // Lagre endringer
                await _context.SaveChangesAsync();
                return updateLike;
            }
            else
            {
                // Legg til ny like i databasen om den ikke eksisterer
                var result = await _context.Likes.AddAsync(like);

                if (result.Entity.PostId != null)
                {
                    // Oppdater post om det er en PostId
                    var post = await _context.Posts.FindAsync(result.Entity.PostId);
                    if (post != null)
                    {
                        post.Like_Count++;
                    }
                }
                else if (result.Entity.CommentId != null)
                {
                    // Oppdater kommentar om det er en CommentId
                    var comment = await _context.Comments.FindAsync(result.Entity.CommentId);
                    if (comment != null)
                    {
                        comment.Like_Count++;
                    }
                }

                // Lagre endringer
                await _context.SaveChangesAsync();
                return result.Entity;
            }
        }

        // DELETE: DeleteLike
        public async Task<Like> DeleteLike(Like like)
        {
            // Finn riktig like
            var deleteLike = await GetLike(like);

            if (deleteLike != null)
            {
                if (deleteLike.PostId != null)
                {
                    // Oppdater post om det er en PostId
                    var post = await _context.Posts.FindAsync(deleteLike.PostId);
                    if (post != null)
                    {
                        post.Like_Count--;
                    }
                }
                else if (deleteLike.CommentId != null)
                {
                    // Oppdater kommentar om det er en CommentId
                    var comment = await _context.Comments.FindAsync(deleteLike.CommentId);
                    if (comment != null)
                    {
                        comment.Like_Count--;
                    }
                }

                // Slett denne fra databasen og lagre endringer
                _context.Likes.Remove(deleteLike);
                await _context.SaveChangesAsync();
                return deleteLike;
            }
            else
            {
                return null;
            }
        }
    }
}
