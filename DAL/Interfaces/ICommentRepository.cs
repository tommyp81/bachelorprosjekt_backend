﻿using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<ICollection<Comment>> GetComments();
        Task<Comment> GetComment(int id);
        Task<Comment> AddComment(IFormFile file, Comment answer);
        Task<Comment> UpdateComment(Comment answer);
        Task<Comment> DeleteComment(int id);
    }
}
