using BLL.Interfaces;
using DAL.Helpers;
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

        public PostBLL(IPostRepository repository)
        {
            _repository = repository;
        }

        private async Task<PostDTO> AddDTO(Post post)
        {
            // Sende med Topic ID i DTO
            var topicId = await _repository.GetTopicId(post.SubTopicId);
            var postDTO = new PostDTO(post)
            {
                TopicId = topicId
            };
            return postDTO;
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

        public async Task<PageResponse<IEnumerable<PostDTO>>> PagedList(int? subTopicId, int page, int size, string order, string type)
        {
            var posts = await _repository.PagedList(subTopicId, page, size, order, type);
            var postDTOs = new List<PostDTO>();
            foreach (var post in posts.Data)
            {
                postDTOs.Add(await AddDTO(post));
            }
            return new PageResponse<IEnumerable<PostDTO>>(postDTOs, posts.Count, subTopicId, page, size, order, type);
        }

        public async Task<PageResponse<IEnumerable<PostDTO>>> Search(string query, int? subTopicId, int page, int size, string order, string type)
        {
            var posts = await _repository.Search(query, subTopicId, page, size, order, type);
            var postDTOs = new List<PostDTO>();
            foreach (var post in posts.Data)
            {
                postDTOs.Add(await AddDTO(post));
            }
            return new PageResponse<IEnumerable<PostDTO>>(postDTOs, posts.Count, subTopicId, page, size, order, type);
        }
    }
}
