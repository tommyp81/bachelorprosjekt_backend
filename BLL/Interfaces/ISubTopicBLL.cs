using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ISubTopicBLL
    {
        SubTopicDTO AddDTO(SubTopic subtopic);
        Task<IEnumerable<SubTopicDTO>> GetSubTopics();
        Task<SubTopicDTO> GetSubTopic(int id);
        Task<SubTopicDTO> AddSubTopic(SubTopic subtopic);
        Task<SubTopicDTO> UpdateSubTopic(SubTopic subtopic);
        Task<SubTopicDTO> DeleteSubTopic(int id);
    }
}
