﻿using DAL.Database_configuration;
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
    public class VideoRepository : IVideoRepository
    {
        private readonly DBContext _context;

        public VideoRepository(DBContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<ICollection<Video>> GetVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        // GET: Videos/1
        public async Task<Video> GetVideo(int id)
        {
            return await _context.Videos.FindAsync(id);
        }

        // POST: Videos
        public async Task<Video> AddVideo(Video video)
        {
            var result = await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: Videos/1
        public async Task<Video> UpdateVideo(Video video)
        {
            var result = await _context.Videos.FindAsync(video.Id);
            if (result != null)
            {
                result.Id = video.Id;
                result.YouTubeId = video.YouTubeId;
                result.Title = video.Title;
                result.Description = video.Description;
                result.UserId = video.UserId;
                result.PostId = video.PostId;
                result.InfoTopic = video.InfoTopic;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: Videos/1
        public async Task<Video> DeleteVideo(int id)
        {
            var result = await _context.Videos.FindAsync(id);
            if (result != null)
            {
                _context.Videos.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
