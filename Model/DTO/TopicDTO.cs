using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public TopicDTO(Topic topic)
        {
            Id = topic.Id;
            Title = topic.Title;
            Description = topic.Description;
            ImageUrl = topic.ImageUrl;
        }
    }
}
