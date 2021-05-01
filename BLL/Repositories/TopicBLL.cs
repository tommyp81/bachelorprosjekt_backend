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

        public async Task<IEnumerable<TopicDTO>> GetTopics()
        {
            var topics = await _repository.GetTopics();
            if (topics != null)
            {
                var topicDTOs = new List<TopicDTO>();
                foreach (Topic topic in topics)
                {
                    topicDTOs.Add(AddDTO(topic));
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
                return AddDTO(getTopic);
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
                return AddDTO(addTopic);
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
                return AddDTO(updateTopic);
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
                return AddDTO(deleteTopic);
            }
            else
            {
                return null;
            }
        }
    }
}
