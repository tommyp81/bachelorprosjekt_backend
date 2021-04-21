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
        private readonly ICustomBLL _customRepository;
        private readonly ISubTopicRepository _subTopicRepository;

        public PostBLL(IPostRepository _repository, ICustomBLL _customRepository, ISubTopicRepository _subTopicRepository)
        {
            this._repository = _repository;
            this._customRepository = _customRepository;
            this._subTopicRepository = _subTopicRepository;
        }

        // For å lage DTOs for Posts
        public async Task<PostDTO> AddDTO(Post post)
        {
            // Hente SubTopic
            var subtopic = await _subTopicRepository.GetSubTopic(post.SubTopicId);

            // Telle hvor mange kommentarer hver enkelt post har
            int commentcount = await _customRepository.GetCommentCount(post.Id);

            // Telle antall likes til hver enkelt post
            int likecount = await _customRepository.GetLikeCount(post.Id, null);

            PostDTO DTO = new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
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
            ICollection<Post> posts = await _repository.GetPosts();
            if (posts == null) { return null; }
            ICollection<PostDTO> postDTOs = new List<PostDTO>();
            foreach (Post post in posts)
            {
                postDTOs.Add(await AddDTO(post));
            }
            return postDTOs;
        }

        public async Task<PostDTO> GetPost(int id)
        {
            Post getPost = await _repository.GetPost(id);
            if (getPost == null) { return null; }
            PostDTO postDTO = await AddDTO(getPost);
            return postDTO;
        }

        public async Task<PostDTO> AddPost(IFormFile file, Post post)
        {
            Post addPost = await _repository.AddPost(file, post);
            if (addPost == null) { return null; }
            PostDTO postDTO = await AddDTO(addPost);
            return postDTO;
        }

        public async Task<PostDTO> UpdatePost(Post post)
        {
            Post updatePost = await _repository.UpdatePost(post);
            if (updatePost == null) { return null; }
            PostDTO postDTO = await AddDTO (updatePost);
            return postDTO;
        }

        public async Task<PostDTO> DeletePost(int id)
        {
            Post deletePost = await _repository.DeletePost(id);
            if (deletePost == null) { return null; }
            PostDTO postDTO = await AddDTO(deletePost);
            return postDTO;
        }
    }
}
