﻿using BLL.Interfaces;
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

        // For å lage DTOs for SubTopics
        public SubTopicDTO AddDTO(SubTopic subtopic)
        {
            var DTO = new SubTopicDTO
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
            var subtopics = await _repository.GetSubTopics();
            if (subtopics == null) { return null; }
            var subtopicDTOs = new List<SubTopicDTO>();
            foreach (SubTopic subtopic in subtopics)
            {
                subtopicDTOs.Add(AddDTO(subtopic));
            }
            return subtopicDTOs;
        }

        public async Task<SubTopicDTO> GetSubTopic(int id)
        {
            var getSubTopic = await _repository.GetSubTopic(id);
            if (getSubTopic == null) { return null; }
            var subtopicDTO = AddDTO(getSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> AddSubTopic(SubTopic subtopic)
        {
            var addSubTopic = await _repository.AddSubTopic(subtopic);
            if (addSubTopic == null) { return null; }
            var subtopicDTO = AddDTO(addSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> UpdateSubTopic(SubTopic subtopic)
        {
            var updateSubTopic = await _repository.UpdateSubTopic(subtopic);
            if (updateSubTopic == null) { return null; }
            var subtopicDTO = AddDTO(updateSubTopic);
            return subtopicDTO;
        }

        public async Task<SubTopicDTO> DeleteSubTopic(int id)
        {
            var deleteSubTopic = await _repository.DeleteSubTopic(id);
            if (deleteSubTopic == null) { return null; }
            var subtopicDTO = AddDTO(deleteSubTopic);
            return subtopicDTO;
        }
    }
}
