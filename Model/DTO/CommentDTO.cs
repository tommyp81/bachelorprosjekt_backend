using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        public bool Edited { get; set; }
        public int Like_Count { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public int PostId { get; set; }
        public int? DocumentId { get; set; }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            Date = comment.Date;
            EditDate = comment.EditDate;
            Edited = comment.Edited;
            Like_Count = comment.Like_Count;
            UserId = comment.UserId;
            PostId = comment.PostId;
            DocumentId = comment.DocumentId;
        }
    }
}
