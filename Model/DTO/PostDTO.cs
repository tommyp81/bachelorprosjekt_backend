using Model.Domain_models;
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
        public DateTime? EditDate { get; set; }
        public bool Edited { get; set; }
        public int Comment_Count { get; set; }
        public int Like_Count { get; set; }
        public string Username { get; set; }
        public int? UserId { get; set; }
        public int? TopicId { get; set; }
        public int SubTopicId { get; set; }
        public int? DocumentId { get; set; }

        public PostDTO(Post post, string username, int? topicId)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            Date = post.Date;
            EditDate = post.EditDate;
            Edited = post.Edited;
            Comment_Count = post.Comment_Count;
            Like_Count = post.Like_Count;
            Username = username;
            UserId = post.UserId;
            TopicId = topicId;
            SubTopicId = post.SubTopicId;
            DocumentId = post.DocumentId;
        }
    }
}
