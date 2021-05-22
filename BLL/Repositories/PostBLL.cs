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

        public async Task<IEnumerable<PostDTO>> GetPosts()
        {
            var posts = await _repository.GetPosts();
            if (posts != null)
            {
                var postDTOs = new List<PostDTO>();
                foreach (var post in posts)
                {
                    var topicId = await _repository.GetTopicId(post.SubTopicId);
                    var username = await _repository.GetUsername(post.UserId);
                    postDTOs.Add(new PostDTO(post, username, topicId));
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
                var topicId = await _repository.GetTopicId(getPost.SubTopicId);
                var username = await _repository.GetUsername(getPost.UserId);
                return new PostDTO(getPost, username, topicId);
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
                var topicId = await _repository.GetTopicId(addPost.SubTopicId);
                var username = await _repository.GetUsername(addPost.UserId);
                return new PostDTO(addPost, username, topicId);
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
                var topicId = await _repository.GetTopicId(updatePost.SubTopicId);
                var username = await _repository.GetUsername(updatePost.UserId);
                return new PostDTO(updatePost, username, topicId);
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
                var topicId = await _repository.GetTopicId(deletePost.SubTopicId);
                var username = await _repository.GetUsername(deletePost.UserId);
                return new PostDTO(deletePost, username, topicId);
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<IEnumerable<PostDTO>>> PagedList(int? subTopicId, int page, int size, string order, string type)
        {
            var posts = await _repository.PagedList(subTopicId, page, size, order, type);
            if (posts != null)
            {
                var postDTOs = new List<PostDTO>();
                foreach (var post in posts.Data)
                {
                    var topicId = await _repository.GetTopicId(post.SubTopicId);
                    var username = await _repository.GetUsername(post.UserId);
                    postDTOs.Add(new PostDTO(post, username, topicId));
                }
                return new PageResponse<IEnumerable<PostDTO>>(postDTOs, posts.Count, subTopicId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<PostDTO>>(null, 0, subTopicId, page, size, order, type);
            }
        }

        public async Task<PageResponse<IEnumerable<PostDTO>>> Search(string query, int? subTopicId, int page, int size, string order, string type)
        {
            var posts = await _repository.Search(query, subTopicId, page, size, order, type);
            if (posts != null)
            {
                var postDTOs = new List<PostDTO>();
                foreach (var post in posts.Data)
                {
                    var topicId = await _repository.GetTopicId(post.SubTopicId);
                    var username = await _repository.GetUsername(post.UserId);
                    postDTOs.Add(new PostDTO(post, username, topicId));
                }
                return new PageResponse<IEnumerable<PostDTO>>(postDTOs, posts.Count, subTopicId, page, size, order, type);
            }
            else
            {
                return new PageResponse<IEnumerable<PostDTO>>(null, 0, subTopicId, page, size, order, type);
            }
        }
    }
}
