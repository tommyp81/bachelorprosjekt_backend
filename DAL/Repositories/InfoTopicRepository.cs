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
    public class InfoTopicRepository: IInfoTopicRepository
    {
        private readonly DBContext _context;

        public InfoTopicRepository(DBContext context)
        {
            _context = context;
        }

        // GET: InfoTopics
        public async Task<ICollection<InfoTopic>> GetInfoTopics()
        {
            return await _context.InfoTopics.ToListAsync();
        }

        // GET: InfoTopics/1
        public async Task<InfoTopic> GetInfoTopic(int id)
        {
            return await _context.InfoTopics.FindAsync(id);
        }

        // POST: InfoTopics
        public async Task<InfoTopic> AddInfoTopic(InfoTopic infotopic)
        {
            var result = await _context.InfoTopics.AddAsync(infotopic);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        // PUT: InfoTopics/1
        public async Task<InfoTopic> UpdateInfoTopic(InfoTopic infotopic)
        {
            var result = await _context.InfoTopics.FindAsync(infotopic.Id);
            if (result != null)
            {
                result.Id = infotopic.Id;
                result.Title = infotopic.Title;
                result.Description = infotopic.Description;
                result.ImageUrl = infotopic.ImageUrl;
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        // DELETE: InfoTopics/1
        public async Task<InfoTopic> DeleteInfoTopic(int id)
        {
            var result = await _context.InfoTopics.FindAsync(id);
            if (result != null)
            {
                _context.InfoTopics.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
