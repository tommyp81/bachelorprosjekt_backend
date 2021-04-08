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

        public PostBLL(IPostRepository _repository, ICustomBLL _customRepository)
        {
            this._repository = _repository;
            this._customRepository = _customRepository;
        }

        // For å lage DTOs for Posts
        public PostDTO AddDTO(Post post)
        {
            PostDTO DTO = new PostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                UserId = post.UserId,
                TopicId = post.TopicId,
                SubTopicId = post.SubTopicId,
                DocumentId = post.DocumentId
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
                // Telle hvor mange kommentarer hver enkelt post har
                int commentcount = await _customRepository.GetCommentCount(post.Id);

                // Telle antall likes til hver enkelt post
                int likecount = await _customRepository.GetLikeCount(post.Id, null);

                // Lag en DTO og legg til feltet for commentcount og likecount
                PostDTO postDTO = AddDTO(post);
                postDTO.Comment_Count = commentcount; // Comment_Count vises kun med DTO
                postDTO.Like_Count = likecount; // Like_Count vises kun med DTO

                // Legg disse til i lista
                postDTOs.Add(postDTO);
            }
            return postDTOs;
        }

        public async Task<PostDTO> GetPost(int id)
        {
            Post getPost = await _repository.GetPost(id);
            if (getPost == null) { return null; }

            // Telle hvor mange kommentarer denne posten har
            int commentcount = await _customRepository.GetCommentCount(getPost.Id);

            // Telle antall likes til denne posten
            int likecount = await _customRepository.GetLikeCount(getPost.Id, null);

            PostDTO postDTO = AddDTO(getPost);
            postDTO.Comment_Count = commentcount; // Comment_Count vises kun med DTO
            postDTO.Like_Count = likecount; // Like_Count vises kun med DTO

            return postDTO;
        }

        public async Task<PostDTO> AddPost(IFormFile file, Post post)
        {
            Post addPost = await _repository.AddPost(file, post);
            if (addPost == null) { return null; }
            PostDTO postDTO = AddDTO(addPost);
            return postDTO;
        }

        public async Task<PostDTO> UpdatePost(Post post)
        {
            Post updatePost = await _repository.UpdatePost(post);
            if (updatePost == null) { return null; }

            // Telle hvor mange kommentarer denne posten har
            int commentcount = await _customRepository.GetCommentCount(updatePost.Id);

            // Telle antall likes til denne posten
            int likecount = await _customRepository.GetLikeCount(updatePost.Id, null);

            PostDTO postDTO = AddDTO(updatePost);
            postDTO.Comment_Count = commentcount; // Comment_Count vises kun med DTO
            postDTO.Like_Count = likecount; // Like_Count vises kun med DTO

            return postDTO;
        }

        public async Task<PostDTO> DeletePost(int id)
        {
            Post deletePost = await _repository.DeletePost(id);
            if (deletePost == null) { return null; }
            PostDTO postDTO = AddDTO(deletePost);
            return postDTO;
        }
    }
}
