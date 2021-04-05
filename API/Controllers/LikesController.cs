﻿using BLL.Interfaces;
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
    [Route("[action]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        // Controller for Likes API Backend

        private readonly ILikeBLL _repository;

        public LikesController(ILikeBLL _repository)
        {
            this._repository = _repository;
        }

        // POST: GetLike
        [HttpPost]
        public async Task<ActionResult<LikeDTO>> GetLike(Like like)
        {
            try
            {
                // Sjekk at nødvendig data er lagt til
                if (like.UserId == null)
                {
                    return BadRequest("Det må legges ved en bruker ID");
                }
                if (like.PostId == null && like.CommentId == null)
                {
                    return BadRequest("Det må legges ved enten post ID eller kommentar ID");
                }
                if (like.PostId != null && like.CommentId != null)
                {
                    return BadRequest("Det kan ikke legges ved både post ID og kommentar ID");
                }

                // Hent status på likes fra databasen
                var getLike = await _repository.GetLike(like);
                if (getLike == null) { return null; }

                return Ok(getLike);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av likes");
            }
        }

        // POST: AddLike
        [HttpPost]
        public async Task<ActionResult<LikeDTO>> AddLike(Like like)
        {
            try
            {
                // Sjekk at nødvendig data er lagt til
                if (like.UserId == null)
                {
                    return BadRequest("Det må legges ved en bruker ID");
                }
                if (like.PostId == null && like.CommentId == null)
                {
                    return BadRequest("Det må legges ved enten post ID eller kommentar ID");
                }
                if (like.PostId != null && like.CommentId != null)
                {
                    return BadRequest("Det kan ikke legges ved både post ID og kommentar ID");
                }

                // Oppdater like i databasen
                var addLike = await _repository.AddLike(like);

                return Ok(addLike);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny like");
            }
        }

        // DELETE: DeleteLike
        [HttpDelete]
        public async Task<ActionResult<LikeDTO>> DeleteLike(Like like)
        {
            try
            {
                // Sjekk at nødvendig data er lagt til
                if (like.UserId == null)
                {
                    return BadRequest("Det må legges ved en bruker ID");
                }
                if (like.PostId == null && like.CommentId == null)
                {
                    return BadRequest("Det må legges ved enten post ID eller kommentar ID");
                }
                if (like.PostId != null && like.CommentId != null)
                {
                    return BadRequest("Det kan ikke legges ved både post ID og kommentar ID");
                }

                // Oppdater like i databasen
                var deleteLike = await _repository.DeleteLike(like);

                return Ok(deleteLike);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av like");
            }
        }
    }
}
