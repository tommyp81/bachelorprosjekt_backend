using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        // Controller for Posts API Backend

        private readonly IPostBLL _repository;

        public PostsController(IPostBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: Posts
        [HttpGet]
        public async Task<ActionResult<ICollection<PostDTO>>> GetPosts()
        {
            try
            {
                return Ok(await _repository.GetPosts());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av poster");
            }
        }

        // GET: Posts/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
            try
            {
                var post = await _repository.GetPost(id);
                if (post == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return post;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av post");
            }
        }

        // POST: Posts
        [HttpPost]
        public async Task<ActionResult<PostDTO>> AddPost([FromForm] IFormFile file, [FromForm] Post post)
        {
            try
            {
                if (post == null)
                {
                    return BadRequest();
                }

                // Legg til posten i databasen og fil på Azure Storage og databasen hvis fil er sendt med
                var newPost = await _repository.AddPost(file, post);

                return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny post");
            }
        }

        // PUT: Posts/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int id, [FromForm] Post post)
        {
            try
            {
                if (id != post.Id)
                {
                    return BadRequest("Post ID stemmer ikke");
                }

                var updatePost = await _repository.GetPost(id);
                if (updatePost == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return await _repository.UpdatePost(post);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av post");
            }
        }

        // DELETE: Posts/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<PostDTO>> DeletePost(int id)
        {
            try
            {
                var deletePost = await _repository.GetPost(id);
                if (deletePost == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return await _repository.DeletePost(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av post");
            }
        }
    }
}
