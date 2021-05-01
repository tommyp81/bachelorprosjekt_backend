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
    public class InfoTopicRepository : IInfoTopicRepository
    {
        private readonly DBContext _context;
        private readonly ICustomRepository _customRepository;
        private readonly IVideoRepository _videoRepository;

        public InfoTopicRepository(DBContext context, ICustomRepository customRepository, IVideoRepository videoRepository)
        {
            _context = context;
            _customRepository = customRepository;
            _videoRepository = videoRepository;
        }

        // GET: InfoTopics
        public async Task<IEnumerable<InfoTopic>> GetInfoTopics()
        {
            var infoTopics = await _context.InfoTopics.ToListAsync();
            if (infoTopics != null)
            {
                return infoTopics;
            }
            else
            {
                return null;
            }
        }

        // GET: InfoTopics/1
        public async Task<InfoTopic> GetInfoTopic(int id)
        {
            var infoTopic = await _context.InfoTopics.FindAsync(id);
            if (infoTopic != null)
            {
                return infoTopic;
            }
            else
            {
                return null;
            }
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
                result.Id = result.Id;
                result.Title = infotopic.Title;
                result.Description = infotopic.Description;
                await _context.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        // DELETE: InfoTopics/1
        public async Task<InfoTopic> DeleteInfoTopic(int id)
        {
            var result = await _context.InfoTopics.FindAsync(id);
            if (result != null)
            {
                // Slett alle dokumenter først
                var documents = await _customRepository.GetDocuments();
                foreach (var document in documents)
                {
                    if (document.InfoTopicId == result.Id)
                    {
                        await _customRepository.DeleteDocument(document.Id);
                    }
                }

                // Slett alle videoer først
                var videos = await _videoRepository.GetVideos();
                foreach (var video in videos)
                {
                    if (video.InfoTopicId == result.Id)
                    {
                        await _videoRepository.DeleteVideo(video.Id);
                    }
                }

                _context.InfoTopics.Remove(result);
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
