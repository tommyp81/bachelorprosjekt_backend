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
        // GetLikes ligger kun i DAL og brukes av Like_Count (CustomBLL)
        Task<ICollection<Like>> GetLikes();

        Task<Like> GetLike(Like like);
        Task<Like> AddLike(Like like);
        Task<Like> DeleteLike(Like like);
    }
}
