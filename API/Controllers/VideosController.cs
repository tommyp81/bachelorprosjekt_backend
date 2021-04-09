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
    public class VideosController : ControllerBase
    {
        // Controller for Videos API Backend

        private readonly IVideoBLL _repository;

        public VideosController(IVideoBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: Videos
        [HttpGet]
        public async Task<ActionResult<ICollection<VideoDTO>>> GetVideos()
        {
            try
            {
                return Ok(await _repository.GetVideos());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av video");
            }
        }

        // GET: Videos/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VideoDTO>> GetVideo(int id)
        {
            try
            {
                var video = await _repository.GetVideo(id);
                if (video == null)
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }
                return Ok(video);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av video");
            }
        }

        // POST: Videos
        [HttpPost]
        public async Task<ActionResult<VideoDTO>> AddVideo(Video video)
        {
            try
            {
                if (video == null)
                {
                    return BadRequest();
                }
                var newVideo = await _repository.AddVideo(video);
                return CreatedAtAction(nameof(GetVideo), new { id = newVideo.Id }, newVideo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny video");
            }
        }

        // PUT: Videos/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<VideoDTO>> UpdateVideo(int id, Video video)
        {
            try
            {
                if (id != video.Id)
                {
                    return BadRequest("Video ID stemmer ikke");
                }
                var updateVideo = await _repository.GetVideo(id);
                if (updateVideo == null)
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }
                return await _repository.UpdateVideo(video);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av video");
            }
        }

        // DELETE: Videos/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<VideoDTO>> DeleteVideo(int id)
        {
            try
            {
                var deleteVideo = await _repository.GetVideo(id);
                if (deleteVideo == null)
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }
                return await _repository.DeleteVideo(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av video");
            }
        }
    }
}
