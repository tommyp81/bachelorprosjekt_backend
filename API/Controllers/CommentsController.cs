using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // Controller for Comments API Backend

        private readonly ICommentBLL _commentBLL;

        public CommentsController(ICommentBLL commentBLL)
        {
            _commentBLL = commentBLL;
        }

        // GET: Comments
        // GET: Comments?postId=1&pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Date
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int? postId, int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste kommentarer med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Date";

                return Ok(await _commentBLL.PagedList(postId, page, size, order, type));

                //var comments = await _commentBLL.GetComments(postId);
                //if (comments != null)
                //{
                //    return Ok(comments);
                //}
                //else
                //{
                //    return NotFound($"Ingen kommentarer ble funnet");
                //}
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av kommentarer");
            }
        }

        // GET: Comments/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            try
            {
                var comment = await _commentBLL.GetComment(id);
                if (comment != null)
                {
                    return Ok(comment);
                }
                else
                {
                    return NotFound($"Kommentar med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av kommentar");
            }
        }

        // POST: Comments
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> AddComment([FromForm] IFormFile file, [FromForm] Comment comment)
        {
            try
            {
                // Legg til kommentaren i databasen og fil på Azure Storage og databasen hvis fil er sendt med
                var newComment = await _commentBLL.AddComment(file, comment);
                if (newComment != null)
                {
                    return CreatedAtAction(nameof(GetComment), new { id = newComment.Id }, newComment);
                }
                else
                {
                    return BadRequest("Kommentar ble ikke opprettet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av kommentar");
            }
        }

        // PUT: Comments/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CommentDTO>> UpdateComment(int id, [FromForm] Comment comment)
        {
            try
            {
                if (id == comment.Id)
                {
                    var updateComment = await _commentBLL.UpdateComment(comment);
                    if (updateComment != null)
                    {
                        return Ok(updateComment);
                    }
                    else
                    {
                        return NotFound($"Kommentar med ID {id} ble ikke funnet");
                    }
                }
                else
                {
                    return BadRequest("Kommentar ID stemmer ikke");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av kommentar");
            }
        }

        // DELETE: Comments/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CommentDTO>> DeleteComment(int id)
        {
            try
            {
                var deleteComment = await _commentBLL.DeleteComment(id);
                if (deleteComment != null)
                {
                    return Ok(deleteComment);
                }
                else
                {
                    return NotFound($"Kommentar med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av kommentar");
            }
        }

        // GET: Comments/Search?query=eksempel tekst
        // GET: Comments/Search?query=eksempel tekst&postId=1&pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Date
        [HttpGet("{Search}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> Search(string query, int? postId, int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste søk i kommentarer med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Date";

                return Ok(await _commentBLL.Search(query, postId, page, size, order, type));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved søk i kommentarer");
            }
        }
    }
}
