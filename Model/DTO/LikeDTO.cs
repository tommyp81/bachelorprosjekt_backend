﻿using System;
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
    }
}
