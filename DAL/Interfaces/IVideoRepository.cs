using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IVideoRepository
    {
        Task<IEnumerable<Video>> GetVideos();
        Task<Video> GetVideo(int id);
        Task<Video> AddVideo(Video video);
        Task<Video> UpdateVideo(Video video);
        Task<Video> DeleteVideo(int id);
    }
}
