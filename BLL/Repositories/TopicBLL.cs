using BLL.Interfaces;
using DAL.Interfaces;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class TopicBLL : ITopicBLL
    {
        private readonly ITopicRepository _repository;

        public TopicBLL(ITopicRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Topics
        public TopicDTO AddDTO(Topic topic)
        {
            var DTO = new TopicDTO
            {
                Id = topic.Id,
                Title = topic.Title,
                Description = topic.Description,
                ImageUrl = topic.ImageUrl
            };
            return DTO;
        }

        public async Task<ICollection<TopicDTO>> GetTopics()
        {
            var topics = await _repository.GetTopics();
            if (topics == null) { return null; }
            var topicDTOs = new List<TopicDTO>();
            foreach (Topic topic in topics)
            {
                topicDTOs.Add(AddDTO(topic));
            }
            return topicDTOs;
        }

        public async Task<TopicDTO> GetTopic(int id)
        {
            var getTopic = await _repository.GetTopic(id);
            if (getTopic == null) { return null; }
            var topicDTO = AddDTO(getTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> AddTopic(Topic topic)
        {
            var addTopic = await _repository.AddTopic(topic);
            if (addTopic == null) { return null; }
            var topicDTO = AddDTO(addTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> UpdateTopic(Topic topic)
        {
            var updateTopic = await _repository.UpdateTopic(topic);
            if (updateTopic == null) { return null; }
            var topicDTO = AddDTO(updateTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> DeleteTopic(int id)
        {
            var deleteTopic = await _repository.DeleteTopic(id);
            if (deleteTopic == null) { return null; }
            var topicDTO = AddDTO(deleteTopic);
            return topicDTO;
        }
    }
}
