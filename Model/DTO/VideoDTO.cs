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
        public int? UserId { get; set; }
        public int PostId { get; set; }
        public int InfoTopicId { get; set; }
    }
}
