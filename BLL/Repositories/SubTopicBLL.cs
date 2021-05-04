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
    public class SubTopicBLL : ISubTopicBLL
    {
        private readonly ISubTopicRepository _repository;

        public SubTopicBLL(ISubTopicRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SubTopicDTO>> GetSubTopics()
        {
            var subtopics = await _repository.GetSubTopics();
            if (subtopics != null)
            {
                var subtopicDTOs = new List<SubTopicDTO>();
                foreach (SubTopic subtopic in subtopics)
                {
                    subtopicDTOs.Add(new SubTopicDTO(subtopic));
                }
                return subtopicDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopicDTO> GetSubTopic(int id)
        {
            var getSubTopic = await _repository.GetSubTopic(id);
            if (getSubTopic != null)
            {
                return new SubTopicDTO(getSubTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopicDTO> AddSubTopic(SubTopic subtopic)
        {
            var addSubTopic = await _repository.AddSubTopic(subtopic);
            if (addSubTopic != null)
            {
                return new SubTopicDTO(addSubTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopicDTO> UpdateSubTopic(SubTopic subtopic)
        {
            var updateSubTopic = await _repository.UpdateSubTopic(subtopic);
            if (updateSubTopic != null)
            {
                return new SubTopicDTO(updateSubTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<SubTopicDTO> DeleteSubTopic(int id)
        {
            var deleteSubTopic = await _repository.DeleteSubTopic(id);
            if (deleteSubTopic != null)
            {
                return new SubTopicDTO(deleteSubTopic);
            }
            else
            {
                return null;
            }
        }
    }
}
