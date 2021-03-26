using BLL.Interfaces;
using DAL.Interfaces;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class LikeBLL : ILikeBLL
    {
        private readonly ILikeRepository _repository;

        public LikeBLL(ILikeRepository _repository)
        {
            this._repository = _repository;
        }

        // For å lage DTOs for Likes
        public LikeDTO AddDTO(Like like)
        {
            LikeDTO DTO = new LikeDTO
            {
                Id = like.Id,
                UserId = like.UserId,
                PostId = like.PostId,
                CommentId = like.CommentId
            };
            return DTO;
        }

        public async Task<LikeDTO> GetLike(Like like)
        {
            Like getLike = await _repository.GetLike(like);
            if (getLike == null) { return null; }
            LikeDTO likeDTO = AddDTO(getLike);
            return likeDTO;
        }

        public async Task<LikeDTO> AddLike(Like like)
        {
            Like addLike = await _repository.AddLike(like);
            if (addLike == null) { return null; }
            LikeDTO likeDTO = AddDTO(addLike);
            return likeDTO;
        }

        public async Task<LikeDTO> DeleteLike(Like like)
        {
            Like deleteLike = await _repository.DeleteLike(like);
            if (deleteLike == null) { return null; }
            LikeDTO likeDTO = AddDTO(deleteLike);
            return likeDTO;
        }
    }
}
