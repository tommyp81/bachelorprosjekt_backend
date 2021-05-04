using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IInfoTopicBLL
    {
        Task<IEnumerable<InfoTopicDTO>> GetInfoTopics();
        Task<InfoTopicDTO> GetInfoTopic(int id);
        Task<InfoTopicDTO> AddInfoTopic(InfoTopic infotopic);
        Task<InfoTopicDTO> UpdateInfoTopic(InfoTopic infotopic);
        Task<InfoTopicDTO> DeleteInfoTopic(int id);
    }
}
