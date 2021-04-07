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
    public class VideoBLL: IVideoBLL
    {
        private readonly IVideoRepository _repository;

        public VideoBLL(IVideoRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for Users
        public VideoDTO AddDTO(Video video)
        {
            VideoDTO DTO = new VideoDTO
            {
                Id = video.Id,
                YouTubeId = video.YouTubeId,
                Title = video.Title,
                Description = video.Description,
                UserId = video.UserId,
                InfoTopicId = video.InfoTopicId
        };
            return DTO;
        }

        public async Task<ICollection<VideoDTO>> GetVideos()
        {
            ICollection<Video> videos = await _repository.GetVideos();
            if (videos == null) { return null; }
            ICollection<VideoDTO> videoDTOs = new List<VideoDTO>();
            foreach (Video video in videos)
            {
                videoDTOs.Add(AddDTO(video));
            }
            return videoDTOs;
        }

        public async Task<VideoDTO> GetVideo(int id)
        {
            Video getVideo = await _repository.GetVideo(id);
            if (getVideo == null) { return null; }
            VideoDTO videoDTO = AddDTO(getVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> AddVideo(Video video)
        {
            Video addVideo = await _repository.AddVideo(video);
            if (addVideo == null) { return null; }
            VideoDTO videoDTO = AddDTO(addVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> UpdateVideo(Video video)
        {
            Video updateVideo = await _repository.UpdateVideo(video);
            if (updateVideo == null) { return null; }
            VideoDTO videoDTO = AddDTO(updateVideo);
            return videoDTO;
        }

        public async Task<VideoDTO> DeleteVideo(int id)
        {
            Video deleteVideo = await _repository.DeleteVideo(id);
            if (deleteVideo == null) { return null; }
            VideoDTO videoDTO = AddDTO(deleteVideo);
            return videoDTO;
        }
    }
}
