using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class InfoTopicObject
    {
        public static ICollection<InfoTopicDTO> TestInfoTopicListDTO()
        {
            var infotopics = new List<InfoTopicDTO>
            {
                new InfoTopicDTO(null)
                {
                    Id = 1,
                    Title="InfoTopic Test 1",
                    Description="Informasjon om InfoTopic Test 1"
                },
                new InfoTopicDTO(null)
                {
                    Id = 2,
                    Title="InfoTopic Test 2",
                    Description="Informasjon om InfoTopic Test 2"
                },
                new InfoTopicDTO(null)
                {
                    Id = 3,
                    Title="InfoTopic Test 3",
                    Description="Informasjon om InfoTopic Test 3"
                },
            };
            return infotopics;
        }

        public static InfoTopicDTO TestInfoTopicDTO()
        {
            var infotopic = new InfoTopicDTO(null)
            {
                Id = 1,
                Title = "InfoTopic Test 1",
                Description = "Informasjon om InfoTopic Test 1"
            };
            return infotopic;
        }

        public static InfoTopic TestInfoTopic()
        {
            var infotopic = new InfoTopic()
            {
                Id = 1,
                Title = "InfoTopic Test 1",
                Description = "Informasjon om InfoTopic Test 1"
            };
            return infotopic;
        }
    }
}
