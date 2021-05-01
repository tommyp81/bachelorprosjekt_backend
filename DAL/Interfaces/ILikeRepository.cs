using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like> GetLike(Like like);
        Task<Like> AddLike(Like like);
        Task<Like> DeleteLike(Like like);
    }
}
