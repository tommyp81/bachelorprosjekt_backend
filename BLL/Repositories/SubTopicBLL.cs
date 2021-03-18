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

        public SubTopicBLL(ISubTopicRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for SubTopics
        public SubTopicDTO AddDTO(SubTopic subtopic)
        {
            SubTopicDTO DTO = new SubTopicDTO
            {
                Id = subtopic.Id,
                Title = subtopic.Title,
                Description = subtopic.Description,
                TopicId = subtopic.TopicId
            };
            return DTO;
        }

        public async Task<ICollection<SubTopicDTO>> GetSubTopics()
        {
            ICollection<SubTopic> subtopics = await _repository.GetSubTopics();
            if (subtopics == null) { return null; }
            ICollection<SubTopicDTO> subtopicDTOs = new List<SubTopicDTO>();
            foreach (SubTopic subtopic in subtopics)
            {
                SubTopic getSubTopic = await _repository.GetSubTopic(subtopic.Id);
                if (getSubTopic == null) { return null; }
                subtopicDTOs.Add(AddDTO(getSubTopic));
            }
            return subtopicDTOs;
        }

        public async Task<SubTopicDTO> GetSubTopic(int id)
        {
            SubTopic getSubTopic = await _repository.GetSubTopic(id);
            if (getSubTopic == null) { return null; }
            SubTopicDTO subtopicDTO = AddDTO(getSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> AddSubTopic(SubTopic subtopic)
        {
            SubTopic addSubTopic = await _repository.AddSubTopic(subtopic);
            if (addSubTopic == null) { return null; }
            SubTopicDTO subtopicDTO = AddDTO(addSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> UpdateSubTopic(SubTopic subtopic)
        {
            SubTopic updateSubTopic = await _repository.UpdateSubTopic(subtopic);
            if (updateSubTopic == null) { return null; }
            SubTopicDTO subtopicDTO = AddDTO(updateSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> DeleteSubTopic(int id)
        {
            SubTopic deleteSubTopic = await _repository.DeleteSubTopic(id);
            if (deleteSubTopic == null) { return null; }
            SubTopicDTO subtopicDTO = AddDTO(deleteSubTopic);
            return subtopicDTO;
        }
    }
}
