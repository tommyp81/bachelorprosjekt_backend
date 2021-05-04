using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class InfoTopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public InfoTopicDTO(InfoTopic infoTopic)
        {
            Id = infoTopic.Id;
            Title = infoTopic.Title;
            Description = infoTopic.Description;
        }
    }
}
