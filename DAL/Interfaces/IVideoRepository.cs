using Model.Domain_models;
using Model.Wrappers;
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
        Task<Response<IEnumerable<Video>>> PagedList(int? infoTopicId, int page, int size, string order, string type);
        Task<Response<IEnumerable<Video>>> Search(string query, int? infoTopicId, int page, int size, string order, string type);
    }
}
