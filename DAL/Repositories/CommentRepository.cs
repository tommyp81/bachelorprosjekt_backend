using DAL.Database_configuration;
using DAL.Helpers;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DBContext _context;
        private readonly ICustomRepository _customRepository;

        public CommentRepository(DBContext context, ICustomRepository customRepository)
        {
            _context = context;
            _customRepository = customRepository;
        }

        // GET: Comments
        // GET: Comments?postId=1
        public async Task<IEnumerable<Comment>> GetComments(int? postId)
        {
            if (postId != null)
            {
                return _context.Comments.Where(c => c.PostId == postId);
            }
            else
            {
                // Alle kommentarer
                return await _context.Comments.ToListAsync();
            }
        }

        // GET: Comments/1
        public async Task<Comment> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                return comment;
            }
            else
            {
                return null;
            }
        }

        // POST: Comments
        public async Task<Comment> AddComment(IFormFile file, Comment comment)
        {
            // Lagre ny kommentar i databasen først
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            comment.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            comment.Edited = false;
            comment.Like_Count = 0;

            var result = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Legge til en comment_count på posten
            var post = await _context.Posts.FindAsync(result.Entity.PostId);
            if (post != null)
            {
                post.Comment_Count++;
            }

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
                    }
                }
            }

            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: Comments/1
        public async Task<Comment> UpdateComment(Comment comment)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var result = await _context.Comments.FindAsync(comment.Id);
            if (result != null)
            {
                result.Id = result.Id;
                result.Content = comment.Content;
                result.Date = result.Date;
                result.EditDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                result.Edited = true;
                result.Like_Count = result.Like_Count;
                result.UserId = result.UserId;
                result.PostId = result.PostId;
                result.DocumentId = result.DocumentId;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        // DELETE: Comments/1
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
                var likes = await _context.Likes.ToListAsync();
                likes.RemoveAll(l => l.CommentId == result.Id);

                // Fjerne Comment_Count fra posten
                var post = await _context.Posts.FindAsync(result.PostId);
                if (post != null)
                {
                    post.Comment_Count--;
                }

                // Lagre alle endringer
                _context.Comments.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Response<IEnumerable<Comment>>> PagedList(int? postId, int page, int size, string order, string type)
        {
            IEnumerable<Comment> list;
            if (postId != null)
            {
                list = await _context.Comments.AsQueryable().Where(q => q.PostId == postId).OrderBy(type + " " + order).ToListAsync();
            }
            else
            {
                list = await _context.Comments.AsQueryable().OrderBy(type + " " + order).ToListAsync();
            }

            var count = list.Count();
            var pagedList = await list.ToPagedListAsync(page, size);
            return new Response<IEnumerable<Comment>>(pagedList, count);
        }

        public async Task<Response<IEnumerable<Comment>>> Search(string query, int? postId, int page, int size, string order, string type)
        {
            if (!string.IsNullOrEmpty(query))
            {
                IEnumerable<Comment> list;
                if (postId != null)
                {
                    list = await _context.Comments.AsQueryable().Where(q => q.PostId == postId).OrderBy(type + " " + order).ToListAsync();
                }
                else
                {
                    list = await _context.Comments.AsQueryable().OrderBy(type + " " + order).ToListAsync();
                }

                var searchList = list.Where(q => q.Content.Contains(query));
                var count = searchList.Count();
                var pagedSearchList = await searchList.ToPagedListAsync(page, size);
                return new Response<IEnumerable<Comment>>(pagedSearchList, count);
            }
            else
            {
                return null;
            }
        }
    }
}
