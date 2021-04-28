using BLL.Interfaces;
using DAL.Interfaces;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class VideoBLL : IVideoBLL
    {
        private readonly IVideoRepository _repository;

        public VideoBLL(IVideoRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Users
        public VideoDTO AddDTO(Video video)
        {
            var DTO = new VideoDTO
            {
                Id = video.Id,
                YouTubeId = video.YouTubeId,
                Title = video.Title,
                Description = video.Description,
                UserId = video.UserId,
                PostId = video.PostId,
                InfoTopicId = video.InfoTopicId
            };
            return DTO;
        }

        public async Task<ICollection<VideoDTO>> GetVideos()
        {
            var videos = await _repository.GetVideos();
            if (videos == null) { return null; }
            var videoDTOs = new List<VideoDTO>();
            foreach (Video video in videos)
            {
                videoDTOs.Add(AddDTO(video));
            }
            return videoDTOs;
        }

        public async Task<VideoDTO> GetVideo(int id)
        {
            var getVideo = await _repository.GetVideo(id);
            if (getVideo == null) { return null; }
            var videoDTO = AddDTO(getVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> AddVideo(Video video)
        {
            var addVideo = await _repository.AddVideo(video);
            if (addVideo == null) { return null; }
            var videoDTO = AddDTO(addVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> UpdateVideo(Video video)
        {
            var updateVideo = await _repository.UpdateVideo(video);
            if (updateVideo == null) { return null; }
            var videoDTO = AddDTO(updateVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> DeleteVideo(int id)
        {
            var deleteVideo = await _repository.DeleteVideo(id);
            if (deleteVideo == null) { return null; }
            var videoDTO = AddDTO(deleteVideo);
            return videoDTO;
        }
    }
}
