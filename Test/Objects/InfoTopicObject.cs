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
            var infotopics = new List<InfoTopic>
            {
                new InfoTopic
                {
                    Id = 1,
                    Title="InfoTopic Test 1",
                    Description="Informasjon om InfoTopic Test 1"
                },
                new InfoTopic
                {
                    Id = 2,
                    Title="InfoTopic Test 2",
                    Description="Informasjon om InfoTopic Test 2"
                },
                new InfoTopic
                {
                    Id = 3,
                    Title="InfoTopic Test 3",
                    Description="Informasjon om InfoTopic Test 3"
                },
            };

            var infotopicDTOs = new List<InfoTopicDTO>();
            foreach (var infotopic in infotopics)
            {
                infotopicDTOs.Add(new InfoTopicDTO(infotopic));
            }
            return infotopicDTOs;
        }

        public static InfoTopicDTO TestInfoTopicDTO()
        {
            var infotopic = new InfoTopic
            {
                Id = 1,
                Title = "InfoTopic Test 1",
                Description = "Informasjon om InfoTopic Test 1"
            };
            return new InfoTopicDTO(infotopic);
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
