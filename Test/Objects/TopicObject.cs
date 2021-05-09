using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class TopicObject
    {
        public static IEnumerable<TopicDTO> TestTopicListDTO()
        {
            var topics = new List<Topic>
            {
                new Topic
                {
                    Id = 1,
                    Title="Topic Test 1",
                    Description="Informasjon om Topic Test 1"
                },
                new Topic
                {
                    Id = 2,
                    Title="Topic Test 2",
                    Description="Informasjon om Topic Test 2"
                },
                new Topic
                {
                    Id = 3,
                    Title="Topic Test 3",
                    Description="Informasjon om Topic Test 3"
                },
            };

            var topicDTOs = new List<TopicDTO>();
            foreach (var topic in topics)
            {
                topicDTOs.Add(new TopicDTO(topic));
            }
            return topicDTOs;
        }

        public static TopicDTO TestTopicDTO()
        {
            var topic = new Topic
            {
                Id = 1,
                Title = "Topic Test 1",
                Description = "Informasjon om Topic Test 1"
            };
            return new TopicDTO(topic);
        }

        public static Topic TestTopic()
        {
            var topic = new Topic()
            {
                Id = 1,
                Title = "Topic Test 1",
                Description = "Informasjon om Topic Test 1"
            };
            return topic;
        }
    }
}
