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

        public LikeBLL(ILikeRepository repository)
        {
            _repository = repository;
        }

        // For å lage DTOs for Likes
        public LikeDTO AddDTO(Like like)
        {
            var DTO = new LikeDTO
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
            var getLike = await _repository.GetLike(like);
            if (getLike != null)
            {
                return AddDTO(getLike);
            }
            else
            {
                return null;
            }
        }

        public async Task<LikeDTO> AddLike(Like like)
        {
            var addLike = await _repository.AddLike(like);
            if (addLike != null)
            {
                return AddDTO(addLike);
            }
            else
            {
                return null;
            }
        }

        public async Task<LikeDTO> DeleteLike(Like like)
        {
            var deleteLike = await _repository.DeleteLike(like);
            if (deleteLike != null)
            {
                return AddDTO(deleteLike);
            }
            else
            {
                return null;
            }
        }
    }
}
