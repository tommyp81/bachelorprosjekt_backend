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
        private readonly ISubTopicRepository _subTopicRepository;

        public TopicRepository(DBContext context, ISubTopicRepository _subTopicRepository)
        {
            _context = context;
            this._subTopicRepository = _subTopicRepository;
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
            // For å legge til en fil, må metoden også få en IFormFile file
            // Koden er kommentert ut her fordi den ikke brukes.
            //if (file != null)
            //{
            //    // Informasjon om filnavn
            //    var fileName = Path.GetFileName(file.FileName);
            //    topic.ImageUrl = "images/" + fileName; // Legg filnavnet til objektet

            //    // Hvis filen finnes fra før, så slett den
            //    if (File.Exists(fileName))
            //    {
            //        File.Delete(fileName);
            //    }

            //    // Lag en ny lokalfil for applikasjonen
            //    using (var localFile = File.OpenWrite("wwwroot/images/" + fileName))
            //    using (var uploadedFile = file.OpenReadStream())
            //    {
            //        // Last opp den nye filen
            //        uploadedFile.CopyTo(localFile);

            //    }
            //}

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
                // Hvis dette emnet har noen underemner, skal disse slettes!
                var subtopics = await _subTopicRepository.GetSubTopics();
                foreach(var subtopic in subtopics)
                {
                    if(subtopic.TopicId == result.Id)
                    {
                        await _subTopicRepository.DeleteSubTopic(subtopic.Id);
                    }
                }

                _context.Topics.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
