using Model.Domain_models;
using Model.DTO;
using Model.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IVideoBLL
    {
        VideoDTO AddDTO(Video video);
        Task<IEnumerable<VideoDTO>> GetVideos();
        Task<VideoDTO> GetVideo(int id);
        Task<VideoDTO> AddVideo(Video video);
        Task<VideoDTO> UpdateVideo(Video video);
        Task<VideoDTO> DeleteVideo(int id);
        Task<PageResponse<IEnumerable<VideoDTO>>> PagedList(int? infoTopicId, int page, int size, string order, string type);
        Task<PageResponse<IEnumerable<VideoDTO>>> Search(string query, int? infoTopicId, int page, int size, string order, string type);
    }
}
