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

        public TopicBLL(ITopicRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for Topics
        public TopicDTO AddDTO(Topic topic)
        {
            TopicDTO DTO = new TopicDTO
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
            ICollection<Topic> topics = await _repository.GetTopics();
            if (topics == null) { return null; }
            ICollection<TopicDTO> topicDTOs = new List<TopicDTO>();
            foreach (Topic topic in topics)
            {
                Topic getTopic = await _repository.GetTopic(topic.Id);
                if (getTopic == null) { return null; }
                topicDTOs.Add(AddDTO(getTopic));
            }
            return topicDTOs;
        }

        public async Task<TopicDTO> GetTopic(int id)
        {
            Topic getTopic = await _repository.GetTopic(id);
            if (getTopic == null) { return null; }
            TopicDTO topicDTO = AddDTO(getTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> AddTopic(Topic topic)
        {
            Topic addTopic = await _repository.AddTopic(topic);
            if (addTopic == null) { return null; }
            TopicDTO topicDTO = AddDTO(addTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> UpdateTopic(Topic topic)
        {
            Topic updateTopic = await _repository.UpdateTopic(topic);
            if (updateTopic == null) { return null; }
            TopicDTO topicDTO = AddDTO(updateTopic);
            return topicDTO;
        }

        public async Task<TopicDTO> DeleteTopic(int id)
        {
            Topic deleteTopic = await _repository.DeleteTopic(id);
            if (deleteTopic == null) { return null; }
            TopicDTO topicDTO = AddDTO(deleteTopic);
            return topicDTO;
        }
    }
}
