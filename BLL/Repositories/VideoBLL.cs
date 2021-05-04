using BLL.Interfaces;
using DAL.Interfaces;
using Model.Domain_models;
using Model.DTO;
using Model.Wrappers;
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
        private readonly ICustomBLL _customBLL;

        public VideoBLL(IVideoRepository repository, ICustomBLL customBLL)
        {
            _repository = repository;
            _customBLL = customBLL;
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

        public async Task<IEnumerable<VideoDTO>> GetVideos()
        {
            var videos = await _repository.GetVideos();
            if (videos != null)
            {
                var videoDTOs = new List<VideoDTO>();
                foreach (Video video in videos)
                {
                    videoDTOs.Add(AddDTO(video));
                }
                return videoDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<VideoDTO> GetVideo(int id)
        {
            var getVideo = await _repository.GetVideo(id);
            if (getVideo != null)
            {
                return AddDTO(getVideo);
            }
            else
            {
                return null;
            }
        }

        public async Task<VideoDTO> AddVideo(Video video)
        {
            var addVideo = await _repository.AddVideo(video);
            if (addVideo != null)
            {
                return AddDTO(addVideo);
            }
            else
            {
                return null;
            }
        }

        public async Task<VideoDTO> UpdateVideo(Video video)
        {
            var updateVideo = await _repository.UpdateVideo(video);
            if (updateVideo != null)
            {
                return AddDTO(updateVideo);
            }
            else
            {
                return null;
            }
        }

        public async Task<VideoDTO> DeleteVideo(int id)
        {
            var deleteVideo = await _repository.DeleteVideo(id);
            if (deleteVideo != null)
            {
                return AddDTO(deleteVideo);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<VideoDTO>>> PagedList(int? infoTopicId, int page, int size, string order, string type)
        {
            var videos = await _repository.PagedList(infoTopicId, page, size, order, type);
            if (videos != null)
            {
                var videoDTOs = new List<VideoDTO>();
                foreach (var video in videos.Data)
                {
                    videoDTOs.Add(AddDTO(video));
                }
                return _customBLL.CreateReponse(videoDTOs, videos.Count, infoTopicId, page, size, order, type);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<VideoDTO>>> Search(string query, int? infoTopicId, int page, int size, string order, string type)
        {
            var videos = await _repository.Search(query, infoTopicId, page, size, order, type);
            if (videos != null)
            {
                var videoDTOs = new List<VideoDTO>();
                foreach (var video in videos.Data)
                {
                    videoDTOs.Add(AddDTO(video));
                }
                return _customBLL.CreateReponse(videoDTOs, videos.Count, infoTopicId, page, size, order, type);
            }
            else
            {
                return null;
            }
        }
    }
}
