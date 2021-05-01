using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITopicBLL
    {
        TopicDTO AddDTO(Topic topic);
        Task<IEnumerable<TopicDTO>> GetTopics();
        Task<TopicDTO> GetTopic(int id);
        Task<TopicDTO> AddTopic(Topic topic);
        Task<TopicDTO> UpdateTopic(Topic topic);
        Task<TopicDTO> DeleteTopic(int id);
    }
}
