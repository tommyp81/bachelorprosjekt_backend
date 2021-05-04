using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class SubTopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }

        public SubTopicDTO(SubTopic subTopic)
        {
            Id = subTopic.Id;
            Title = subTopic.Title;
            Description = subTopic.Description;
            TopicId = subTopic.TopicId;
        }
    }
}
