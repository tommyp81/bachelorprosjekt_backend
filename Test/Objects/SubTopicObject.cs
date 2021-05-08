using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class SubTopicObject
    {
        public static IEnumerable<SubTopicDTO> TestSubTopicListDTO()
        {
            var subtopics = new List<SubTopic>
            {
                new SubTopic
                {
                    Id = 1,
                    Title="SubTopic Test 1",
                    Description="Informasjon om SubTopic Test 1"
                },
                new SubTopic
                {
                    Id = 2,
                    Title="SubTopic Test 2",
                    Description="Informasjon om SubTopic Test 2"
                },
                new SubTopic
                {
                    Id = 3,
                    Title="SubTopic Test 3",
                    Description="Informasjon om SubTopic Test 3"
                },
            };

            var subtopicDTOs = new List<SubTopicDTO>();
            foreach (var subtopic in subtopics)
            {
                subtopicDTOs.Add(new SubTopicDTO(subtopic));
            }
            return subtopicDTOs;
        }

        public static SubTopicDTO TestSubTopicDTO()
        {
            var subtopic = new SubTopic
            {
                Id = 1,
                Title = "SubTopic Test 1",
                Description = "Informasjon om SubTopic Test 1"
            };
            return new SubTopicDTO(subtopic);
        }

        public static SubTopic TestSubTopic()
        {
            var subtopic = new SubTopic()
            {
                Id = 1,
                Title = "SubTopic Test 1",
                Description = "Informasjon om SubTopic Test 1"
            };
            return subtopic;
        }
    }
}
