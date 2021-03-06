using DAL.Database_configuration;
using DAL.Helpers;
using DAL.Interfaces;
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
    public class VideoRepository : IVideoRepository
    {
        private readonly DBContext _context;
        private readonly IPostRepository _postRepository;

        public VideoRepository(DBContext context, IPostRepository postRepository)
        {
            _context = context;
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<Video> GetVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                return video;
            }
            else
            {
                return null;
            }
        }

        public async Task<Video> AddVideo(Video video)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            video.Uploaded = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            var result = await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Video> UpdateVideo(Video video)
        {
            var result = await _context.Videos.FindAsync(video.Id);
            if (result != null)
            {
                result.Id = result.Id;
                result.YouTubeId = video.YouTubeId;
                result.Title = video.Title;
                result.Description = video.Description;
                result.UserId = result.UserId;
                result.PostId = result.PostId;
                result.InfoTopicId = result.InfoTopicId;
                result.Uploaded = result.Uploaded;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Video> DeleteVideo(int id)
        {
            var result = await _context.Videos.FindAsync(id);
            if (result != null)
            {
                // Slette den posten som ble laget for denne videoen
                var post = await _postRepository.GetPost(result.PostId);
                if (post != null)
                {
                    // Slett posten, så vil posten fjærne denne videoen!
                    await _postRepository.DeletePost(post.Id);
                    return result;
                }

                // Fjerne video fra databasen om posten ikke finnes
                _context.Videos.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<Response<IEnumerable<Video>>> PagedList(int? infoTopicId, int page, int size, string order, string type)
        {
            IEnumerable<Video> list;
            if (infoTopicId != null)
            {
                list = await _context.Videos.AsQueryable().Where(q => q.InfoTopicId == infoTopicId).OrderBy(type + " " + order).ToListAsync();
            }
            else
            {
                list = await _context.Videos.AsQueryable().OrderBy(type + " " + order).ToListAsync();
            }

            var count = list.Count();
            var pagedList = await list.ToPagedListAsync(page, size);
            return new Response<IEnumerable<Video>>(pagedList, count);
        }

        public async Task<Response<IEnumerable<Video>>> Search(string query, int? infoTopicId, int page, int size, string order, string type)
        {
            if (!string.IsNullOrEmpty(query))
            {
                IEnumerable<Video> list;
                if (infoTopicId != null)
                {
                    list = await _context.Videos.AsQueryable().Where(q => q.InfoTopicId == infoTopicId).OrderBy(type + " " + order).ToListAsync();
                }
                else
                {
                    list = await _context.Videos.AsQueryable().OrderBy(type + " " + order).ToListAsync();
                }

                var searchList = list.Where(q => q.Title.ToLower().Contains(query.ToLower()) || q.Description.ToLower().Contains(query.ToLower()));
                var count = searchList.Count();
                var pagedSearchList = await searchList.ToPagedListAsync(page, size);
                return new Response<IEnumerable<Video>>(pagedSearchList, count);
            }
            else
            {
                return null;
            }
        }
    }
}
