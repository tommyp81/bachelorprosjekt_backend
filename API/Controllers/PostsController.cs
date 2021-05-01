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
        //[Route("{page?}?{count?}?{order?}?{type?}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts() //int? page, int? count, string order, string type
        {
            try
            {
                var posts = await _postBLL.GetPosts();
                if (posts != null)
                {
                    return Ok(posts);
                }
                else
                {
                    return NotFound($"Ingen poster ble funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av poster");
            }

            //try
            //{
            //    //return Ok(await _postBLL.PostPaging(page, count, order, type));
            //    return Ok(await _postBLL.GetPosts());
            //}
            //catch (Exception e)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, e);//"Feil ved henting av poster"
            //}
        }

        // GET: Posts/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
            try
            {
                var post = await _postBLL.GetPost(id);
                if (post != null)
                {
                    return Ok(post);
                }
                else
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }
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
                if (post != null)
                {
                    // Legg til posten i databasen og fil på Azure Storage og databasen hvis fil er sendt med
                    var newPost = await _postBLL.AddPost(file, post);
                    if (newPost != null)
                    {
                        return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
                    }
                    else
                    {
                        return BadRequest("Post ble ikke opprettet");
                    }
                }
                else
                {
                    return BadRequest("Post mangler");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av post");
            }
        }

        // PUT: Posts/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int id, [FromForm] Post post)
        {
            try
            {
                if (id == post.Id)
                {
                    var updatePost = await _postBLL.UpdatePost(post);
                    if (updatePost != null)
                    {
                        return Ok(updatePost);
                    }
                    else
                    {
                        return NotFound($"Post med ID {id} ble ikke funnet");
                    }
                }
                else
                {
                    return BadRequest("Post ID stemmer ikke");
                }
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
                var deletePost = await _postBLL.DeletePost(id);
                if (deletePost != null)
                {
                    return Ok(deletePost);

                }
                else
                {
                    return NotFound($"Post med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av post");
            }
        }
    }
}
