using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILikeBLL
    {
        LikeDTO AddDTO(Like like);
        Task<LikeDTO> GetLike(Like like);
        Task<LikeDTO> AddLike(Like like);
        Task<LikeDTO> DeleteLike(Like like);
    }
}
