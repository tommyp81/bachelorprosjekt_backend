using DAL.Helpers;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class VideoObject
    {
        public static PageResponse<IEnumerable<VideoDTO>> TestPagedResponse(string query)
        {
            var videos = new List<Video>
            {
                new Video
                {
                    Id = 1,
                    YouTubeId = "test1",
                    Title = "Video Test 1",
                    Description = "Informasjon om Video Test 1",
                    InfoTopicId = 1
                },
                new Video
                {
                    Id = 2,
                    YouTubeId = "test2",
                    Title = "Video Test 2",
                    Description = "Informasjon om Video Test 2",
                    InfoTopicId = 2
                },
                new Video
                {
                    Id = 3,
                    YouTubeId = "test3",
                    Title = "Video Test 3",
                    Description = "Informasjon om Video Test 3",
                    InfoTopicId = 3
                },
            };

            var videoDTOs = new List<VideoDTO>();
            foreach (var video in videos)
            {
                videoDTOs.Add(new VideoDTO(video));
            }

            if (string.IsNullOrEmpty(query))
            {
                return new PageResponse<IEnumerable<VideoDTO>>(videoDTOs);
            }
            else
            {
                var searchResult = videoDTOs.AsQueryable().Where(q => q.Title.Contains(query));
                return new PageResponse<IEnumerable<VideoDTO>>(searchResult);
            }
        }

        public static VideoDTO TestVideoDTO()
        {
            var video = new Video
            {
                Id = 1,
                YouTubeId = "test1",
                Title = "Video Test 1",
                Description = "Informasjon om Video Test 1",
                InfoTopicId = 1
            };
            return new VideoDTO(video);
        }

        public static Video TestVideo()
        {
            var video = new Video()
            {
                Id = 1,
                YouTubeId = "test1",
                Title = "Video Test 1",
                Description = "Informasjon om Video Test 1",
                InfoTopicId = 1
            };
            return video;
        }
    }
}
