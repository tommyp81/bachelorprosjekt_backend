using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Objects
{
    public class LikeObject
    {
        public static LikeDTO TestLikeDTO()
        {
            var like = new Like()
            {
                Id = 1,
                UserId = 1,
                PostId = 1
            };
            return new LikeDTO(like);
        }

        public static Like TestLike()
        {
            var like = new Like()
            {
                Id = 1,
                UserId = 1,
                PostId = 1
            };
            return like;
        }

        public static Like TestLike2()
        {
            var like = new Like()
            {
                PostId = 2
            };
            return like;
        }
    }
}
