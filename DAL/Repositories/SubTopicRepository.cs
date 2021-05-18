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
    public class SubTopicRepository : ISubTopicRepository
    {
        private readonly DBContext _context;
        private readonly IPostRepository _postRepository;

        public SubTopicRepository(DBContext context, IPostRepository postRepository)
        {
            _context = context;
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<SubTopic>> GetSubTopics()
        {
            return await _context.SubTopics.ToListAsync();
        }

        public async Task<SubTopic> GetSubTopic(int id)
        {
            var subTopic = await _context.SubTopics.FindAsync(id);
            if (subTopic != null)
            {
                return subTopic;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopic> AddSubTopic(SubTopic subtopic)
        {
            var result = await _context.SubTopics.AddAsync(subtopic);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SubTopic> UpdateSubTopic(SubTopic subtopic)
        {
            var result = await _context.SubTopics.FindAsync(subtopic.Id);
            if (result != null)
            {
                result.Id = result.Id;
                result.Title = subtopic.Title;
                result.Description = subtopic.Description;
                result.TopicId = result.TopicId;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopic> DeleteSubTopic(int id)
        {
            var result = await _context.SubTopics.FindAsync(id);
            if (result != null)
            {
                // Hvis dette underemnet har noen poster, skal disse slettes!
                var posts = await _postRepository.GetPosts();
                foreach (var post in posts)
                {
                    if (post.SubTopicId == result.Id)
                    {
                        await _postRepository.DeletePost(post.Id);
                    }
                }

                _context.SubTopics.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
