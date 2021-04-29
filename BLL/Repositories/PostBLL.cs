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

namespace BLL.Repositories
{
    public class PostBLL : IPostBLL
    {
        private readonly IPostRepository _repository;
        private readonly ICustomBLL _customBLL;
        private readonly ISubTopicBLL _subTopicBLL;

        public PostBLL(IPostRepository repository, ICustomBLL customBLL, ISubTopicBLL subTopicBLL)
        {
            _repository = repository;
            _customBLL = customBLL;
            _subTopicBLL = subTopicBLL;
        }

        // For å lage DTOs for Posts
        public async Task<PostDTO> AddDTO(Post post)
        {
            // Hente SubTopic
            var subtopic = await _subTopicBLL.GetSubTopic(post.SubTopicId);

            // Telle hvor mange kommentarer hver enkelt post har
            int commentcount = await _customBLL.GetCommentCount(post.Id);

            // Telle antall likes til hver enkelt post
            int likecount = await _customBLL.GetLikeCount(post.Id, null);

            var DTO = new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Edited = post.Edited,
                UserId = post.UserId,
                TopicId = subtopic.TopicId, // Hentes fra SubTopic og vises kun med DTO
                SubTopicId = post.SubTopicId,
                DocumentId = post.DocumentId,
                Comment_Count = commentcount, // Comment_Count vises kun med DTO
                Like_Count = likecount // Like_Count vises kun med DTO
            };
            return DTO;
        }

        public async Task<ICollection<PostDTO>> GetPosts()
        {
            var posts = await _repository.GetPosts();
            if (posts == null) { return null; }
            var postDTOs = new List<PostDTO>();
            foreach (Post post in posts)
            {
                postDTOs.Add(await AddDTO(post));
            }
            return postDTOs;
        }

        public async Task<PostDTO> GetPost(int id)
        {
            var getPost = await _repository.GetPost(id);
            if (getPost == null) { return null; }
            var postDTO = await AddDTO(getPost);
            return postDTO;
        }

        public async Task<PostDTO> AddPost(IFormFile file, Post post)
        {
            var addPost = await _repository.AddPost(file, post);
            if (addPost == null) { return null; }
            var postDTO = await AddDTO(addPost);
            return postDTO;
        }

        public async Task<PostDTO> UpdatePost(Post post)
        {
            var updatePost = await _repository.UpdatePost(post);
            if (updatePost == null) { return null; }
            var postDTO = await AddDTO(updatePost);
            return postDTO;
        }

        public async Task<PostDTO> DeletePost(int id)
        {
            var deletePost = await _repository.DeletePost(id);
            if (deletePost == null) { return null; }
            var postDTO = await AddDTO(deletePost);
            return postDTO;
        }
    }
}
