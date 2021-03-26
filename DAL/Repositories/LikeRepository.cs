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

        // GET: GetLikes
        public async Task<ICollection<Like>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        // GET: GetLike
        public async Task<Like> GetLike(Like like)
        {
            // Finne likes etter userId og postId eller commentId
            ICollection<Like> allLikes = await _context.Likes.ToListAsync();
            ICollection<Like> likesByUser = new List<Like>();

            // Lag en ny liste over likes med riktig userId
            foreach (var item in allLikes)
            {
                if (item.UserId == like.UserId)
                {
                    likesByUser.Add(item);
                }
            }

            // Finn like på postId fra likesByUser
            if (like.PostId != null)
            {
                foreach (var item in likesByUser)
                {
                    if (item.PostId == like.PostId)
                    {
                        // Send tilbake like med riktig userId og postId
                        return item;
                    }
                }
            }

            // Finn like på commentId fra likesByUser
            if (like.CommentId != null)
            {
                foreach (var item in likesByUser)
                {
                    if (item.CommentId == like.CommentId)
                    {
                        // Send tilbake like med riktig userId og commentId
                        return item;
                    }
                }
            }

            return null;
        }

        // POST: AddLike
        public async Task<Like> AddLike(Like like)
        {
            // Sjekk om denne eksisterer
            var getLike = await GetLike(like);

            // Oppdater om den finnes
            if (getLike != null)
            {
                var updateLike = await _context.Likes.FindAsync(getLike.Id);
                updateLike.Id = getLike.Id;
                updateLike.PostId = like.PostId;
                updateLike.CommentId = like.CommentId;
                await _context.SaveChangesAsync();
                return updateLike;
            }

            // Lagre endringer til databasen
            var result = await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // DELETE: DeleteLike
        public async Task<Like> DeleteLike(Like like)
        {
            // Finn riktig like
            Like deleteLike = await GetLike(like);

            if (deleteLike != null)
            {
                // Slett denne fra databasen og lagre endringer
                _context.Likes.Remove(deleteLike);
                await _context.SaveChangesAsync();
                return deleteLike;
            }

            return null;
        }
    }
}
