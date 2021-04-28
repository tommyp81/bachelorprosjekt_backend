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

        private readonly IPostBLL _postBLL;

        public PostsController(IPostBLL postBLL)
        {
            _postBLL = postBLL;
        }

        // GET: Posts
        [HttpGet]
        public async Task<ActionResult<ICollection<PostDTO>>> GetPosts()
        {
            try
            {
                return Ok(await _postBLL.GetPosts());
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
                var post = await _postBLL.GetPost(id);
                if (post == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return Ok(post);
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
                var newPost = await _postBLL.AddPost(file, post);
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

                var checkPost = await _postBLL.GetPost(id);
                if (checkPost == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return Ok(await _postBLL.UpdatePost(post));
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
                var checkPost = await _postBLL.GetPost(id);
                if (checkPost == null)
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }

                return Ok(await _postBLL.DeletePost(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av post");
            }
        }
    }
}
