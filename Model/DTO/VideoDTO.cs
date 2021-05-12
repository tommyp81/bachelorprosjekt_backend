using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string YouTubeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Uploaded { get; set; }
        public int? UserId { get; set; }
        public int PostId { get; set; }
        public int InfoTopicId { get; set; }

        public VideoDTO(Video video)
        {
            Id = video.Id;
            YouTubeId = video.YouTubeId;
            Title = video.Title;
            Description = video.Description;
            UserId = video.UserId;
            PostId = video.PostId;
            InfoTopicId = video.InfoTopicId;
            Uploaded = video.Uploaded;
        }
    }
}
