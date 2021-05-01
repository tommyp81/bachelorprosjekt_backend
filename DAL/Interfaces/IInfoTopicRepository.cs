using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IInfoTopicRepository
    {
        Task<IEnumerable<InfoTopic>> GetInfoTopics();
        Task<InfoTopic> GetInfoTopic(int id);
        Task<InfoTopic> AddInfoTopic(InfoTopic infotopic);
        Task<InfoTopic> UpdateInfoTopic(InfoTopic infotopic);
        Task<InfoTopic> DeleteInfoTopic(int id);
    }
}
