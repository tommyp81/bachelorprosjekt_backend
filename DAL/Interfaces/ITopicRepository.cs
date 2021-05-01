using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetTopics();
        Task<Topic> GetTopic(int id);
        Task<Topic> AddTopic(Topic topic);
        Task<Topic> UpdateTopic(Topic topic);
        Task<Topic> DeleteTopic(int id);
    }
}
