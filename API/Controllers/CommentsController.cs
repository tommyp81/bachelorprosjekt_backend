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
    public class CommentsController : ControllerBase
    {
        // Controller for Comments API Backend

        private readonly ICommentBLL _repository;

        public CommentsController(ICommentBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: Comments
        [HttpGet]
        public async Task<ActionResult<ICollection<CommentDTO>>> GetComments()
        {
            try
            {
                return Ok(await _repository.GetComments());
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
                var comment = await _repository.GetComment(id);
                if (comment == null)
                {
                    return NotFound($"Kommentar med ID {id} ble ikke funnet");
                }

                return comment;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av kommentarer");
            }
        }

        // POST: Comments
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> AddComment([FromForm] IFormFile file, [FromForm] Comment comment)
        {
            try
            {
                if (comment == null)
                {
                    return BadRequest();
                }

                // Legg til kommentaren i databasen og fil på Azure Storage og databasen hvis fil er sendt med
                var newComment = await _repository.AddComment(file, comment);

                return CreatedAtAction(nameof(GetComment), new { id = newComment.Id }, newComment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny kommentar");
            }
        }

        // PUT: Comments/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CommentDTO>> UpdateComment(int id, [FromForm] Comment comment)
        {
            try
            {
                if (id != comment.Id)
                {
                    return BadRequest("Kommentar ID stemmer ikke");
                }

                var updateComment = await _repository.GetComment(id);
                if (updateComment == null)
                {
                    return NotFound($"Kommentar med ID {id} ble ikke funnet");
                }

                return await _repository.UpdateComment(comment);
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
                var deleteComment = await _repository.GetComment(id);
                if (deleteComment == null)
                {
                    return NotFound($"Kommentar med ID {id} ble ikke funnet");
                }

                return await _repository.DeleteComment(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av kommentar");
            }
        }
    }
}
