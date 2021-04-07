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
    public class InfoTopicBLL : IInfoTopicBLL
    {
        private readonly IInfoTopicRepository _repository;

        public InfoTopicBLL(IInfoTopicRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for Topics
        public InfoTopicDTO AddDTO(InfoTopic infotopic)
        {
            InfoTopicDTO DTO = new InfoTopicDTO
            {
                Id = infotopic.Id,
                Title = infotopic.Title,
                Description = infotopic.Description,
                ImageUrl = infotopic.ImageUrl
            };
            return DTO;
        }

        public async Task<ICollection<InfoTopicDTO>> GetInfoTopics()
        {
            ICollection<InfoTopic> infotopics = await _repository.GetInfoTopics();
            if (infotopics == null) { return null; }
            ICollection<InfoTopicDTO> infotopicDTOs = new List<InfoTopicDTO>();
            foreach (InfoTopic infotopic in infotopics)
            {
                infotopicDTOs.Add(AddDTO(infotopic));
            }
            return infotopicDTOs;
        }

        public async Task<InfoTopicDTO> GetInfoTopic(int id)
        {
            InfoTopic getInfoTopic = await _repository.GetInfoTopic(id);
            if (getInfoTopic == null) { return null; }
            InfoTopicDTO infotopicDTO = AddDTO(getInfoTopic);
            return infotopicDTO;
        }

        public async Task<InfoTopicDTO> AddInfoTopic(InfoTopic infotopic)
        {
            InfoTopic addInfoTopic = await _repository.AddInfoTopic(infotopic);
            if (addInfoTopic == null) { return null; }
            InfoTopicDTO infotopicDTO = AddDTO(addInfoTopic);
            return infotopicDTO;
        }

        public async Task<InfoTopicDTO> UpdateInfoTopic(InfoTopic infotopic)
        {
            InfoTopic updateInfoTopic = await _repository.UpdateInfoTopic(infotopic);
            if (updateInfoTopic == null) { return null; }
            InfoTopicDTO infotopicDTO = AddDTO(updateInfoTopic);
            return infotopicDTO;
        }

        public async Task<InfoTopicDTO> DeleteInfoTopic(int id)
        {
            InfoTopic deleteInfoTopic = await _repository.DeleteInfoTopic(id);
            if (deleteInfoTopic == null) { return null; }
            InfoTopicDTO infotopicDTO = AddDTO(deleteInfoTopic);
            return infotopicDTO;
        }
    }
}
