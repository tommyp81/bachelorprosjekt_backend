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

        public InfoTopicBLL(IInfoTopicRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Topics
        public InfoTopicDTO AddDTO(InfoTopic infotopic)
        {
            var DTO = new InfoTopicDTO
            {
                Id = infotopic.Id,
                Title = infotopic.Title,
                Description = infotopic.Description
            };
            return DTO;
        }

        public async Task<IEnumerable<InfoTopicDTO>> GetInfoTopics()
        {
            var infotopics = await _repository.GetInfoTopics();
            if (infotopics != null)
            {
                var infotopicDTOs = new List<InfoTopicDTO>();
                foreach (InfoTopic infotopic in infotopics)
                {
                    infotopicDTOs.Add(AddDTO(infotopic));
                }
                return infotopicDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<InfoTopicDTO> GetInfoTopic(int id)
        {
            var getInfoTopic = await _repository.GetInfoTopic(id);
            if (getInfoTopic != null)
            {
                return AddDTO(getInfoTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<InfoTopicDTO> AddInfoTopic(InfoTopic infotopic)
        {
            var addInfoTopic = await _repository.AddInfoTopic(infotopic);
            if (addInfoTopic != null)
            {
                return AddDTO(addInfoTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<InfoTopicDTO> UpdateInfoTopic(InfoTopic infotopic)
        {
            var updateInfoTopic = await _repository.UpdateInfoTopic(infotopic);
            if (updateInfoTopic != null)
            {
                return AddDTO(updateInfoTopic);
            }
            else
            {
                return null;
            }
        }

        public async Task<InfoTopicDTO> DeleteInfoTopic(int id)
        {
            var deleteInfoTopic = await _repository.DeleteInfoTopic(id);
            if (deleteInfoTopic != null)
            {
                return AddDTO(deleteInfoTopic);
            }
            else
            {
                return null;
            }
        }
    }
}
