using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace BLL.Repositories
{
    public class PostBLL : IPostBLL
    {
        private readonly IPostRepository _repository;
        private readonly ISubTopicBLL _subTopicBLL;

        public PostBLL(IPostRepository repository, ISubTopicBLL subTopicBLL)
        {
            _repository = repository;
            _subTopicBLL = subTopicBLL;
        }

        // For å lage DTOs for Posts
        public async Task<PostDTO> AddDTO(Post post)
        {
            // Hente SubTopic
            var subtopic = await _subTopicBLL.GetSubTopic(post.SubTopicId);

            var DTO = new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                EditDate = post.EditDate,
                Edited = post.Edited,
                Comment_Count = post.Comment_Count,
                Like_Count = post.Like_Count,
                UserId = post.UserId,
                TopicId = subtopic.TopicId, // Hentes fra SubTopic og vises kun med DTO
                SubTopicId = post.SubTopicId,
                DocumentId = post.DocumentId
            };
            return DTO;
        }

        public async Task<IEnumerable<PostDTO>> GetPosts()
        {
            var posts = await _repository.GetPosts();
            if (posts != null)
            {
                var postDTOs = new List<PostDTO>();
                foreach (var post in posts)
                {
                    postDTOs.Add(await AddDTO(post));
                }
                return postDTOs;
            }
            else
            {
                return null;
            }
        }

        public async Task<PostDTO> GetPost(int id)
        {
            var getPost = await _repository.GetPost(id);
            if (getPost != null)
            {
                return await AddDTO(getPost);
            }
            else
            {
                return null;
            }
        }

        public async Task<PostDTO> AddPost(IFormFile file, Post post)
        {
            var addPost = await _repository.AddPost(file, post);
            if (addPost != null)
            {
                return await AddDTO(addPost);
            }
            else
            {
                return null;
            }
        }

        public async Task<PostDTO> UpdatePost(Post post)
        {
            var updatePost = await _repository.UpdatePost(post);
            if (updatePost != null)
            {
                return await AddDTO(updatePost);
            }
            else
            {
                return null;
            }
        }

        public async Task<PostDTO> DeletePost(int id)
        {
            var deletePost = await _repository.DeletePost(id);
            if (deletePost != null)
            {
                return await AddDTO(deletePost);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<PostDTO>> PostPaging(int? page, int? count, string order, string type)
        {
            var posts = await _repository.PostPaging(page, count, order, type);
            if (posts != null)
            {
                var postDTOs = new List<PostDTO>();
                foreach (var post in posts)
                {
                    postDTOs.Add(await AddDTO(post));
                }
                return postDTOs;
            }
            else
            {
                return null;
            }
        }
    }
}
