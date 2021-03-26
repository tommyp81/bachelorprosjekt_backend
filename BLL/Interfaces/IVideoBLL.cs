using Model.Domain_models;
using Model.DTO;
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
        Task<ICollection<VideoDTO>> GetVideos();
        Task<VideoDTO> GetVideo(int id);
        Task<VideoDTO> AddVideo(Video video);
        Task<VideoDTO> UpdateVideo(Video video);
        Task<VideoDTO> DeleteVideo(int id);
    }
}
