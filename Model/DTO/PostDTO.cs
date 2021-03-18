using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public int? TopicId { get; set; }
        public int SubTopicId { get; set; }
        public int? DocumentId { get; set; }
        public int Comment_Count { get; set; }
    }
}
