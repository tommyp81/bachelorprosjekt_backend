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
    public class TopicRepository : ITopicRepository
    {
        private readonly DBContext _context;

        public TopicRepository(DBContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<ICollection<Topic>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        // GET: Topics/1
        public async Task<Topic> GetTopic(int id)
        {
            return await _context.Topics.FindAsync(id);
        }

        // POST: Topics
        public async Task<Topic> AddTopic(Topic topic)
        {
            var result = await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: Topics/1
        public async Task<Topic> UpdateTopic(Topic topic)
        {
            var result = await _context.Topics.FindAsync(topic.Id);
            if (result != null)
            {
                result.Id = topic.Id;
                result.Title = topic.Title;
                result.Description = topic.Description;
                result.ImageUrl = topic.ImageUrl;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: Topics/1
        public async Task<Topic> DeleteTopic(int id)
        {
            var result = await _context.Topics.FindAsync(id);
            if (result != null)
            {
                _context.Topics.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
