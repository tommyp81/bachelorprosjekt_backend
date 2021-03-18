using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ISubTopicRepository
    {
        Task<ICollection<SubTopic>> GetSubTopics();
        Task<SubTopic> GetSubTopic(int id);
        Task<SubTopic> AddSubTopic(SubTopic subtopic);
        Task<SubTopic> UpdateSubTopic(SubTopic subtopic);
        Task<SubTopic> DeleteSubTopic(int id);
    }
}
