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

        public async Task<LikeDTO> GetLike(Like like)
        {
            var getLike = await _repository.GetLike(like);
            if (getLike != null)
            {
                return new LikeDTO(getLike);
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
                return new LikeDTO(addLike);
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
                return new LikeDTO(deleteLike);
            }
            else
            {
                return null;
            }
        }
    }
}
