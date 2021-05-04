using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }

        public LikeDTO(Like like)
        {
            Id = like.Id;
            UserId = like.UserId;
            PostId = like.PostId;
            CommentId = like.CommentId;
        }
    }
}
