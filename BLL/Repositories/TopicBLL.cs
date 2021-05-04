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

        public async Task<IEnumerable<TopicDTO>> GetTopics()
        {
            var topics = await _repository.GetTopics();
            if (topics != null)
            {
                var topicDTOs = new List<TopicDTO>();
                foreach (Topic topic in topics)
                {
                    topicDTOs.Add(new TopicDTO(topic));
                }
                return topicDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<TopicDTO> GetTopic(int id)
        {
            var getTopic = await _repository.GetTopic(id);
            if (getTopic != null)
            {
                return new TopicDTO(getTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<TopicDTO> AddTopic(Topic topic)
        {
            var addTopic = await _repository.AddTopic(topic);
            if (addTopic != null)
            {
                return new TopicDTO(addTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<TopicDTO> UpdateTopic(Topic topic)
        {
            var updateTopic = await _repository.UpdateTopic(topic);
            if (updateTopic != null)
            {
                return new TopicDTO(updateTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<TopicDTO> DeleteTopic(int id)
        {
            var deleteTopic = await _repository.DeleteTopic(id);
            if (deleteTopic != null)
            {
                return new TopicDTO(deleteTopic);
            }
            else
            {
                return null;
            }
        }
    }
}
