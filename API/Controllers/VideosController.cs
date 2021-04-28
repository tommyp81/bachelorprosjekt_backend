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

        private readonly IVideoBLL _videoBLL;

        public VideosController(IVideoBLL videoBLL)
        {
            _videoBLL = videoBLL;
        }

        // GET: Videos
        [HttpGet]
        public async Task<ActionResult<ICollection<VideoDTO>>> GetVideos()
        {
            try
            {
                return Ok(await _videoBLL.GetVideos());
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
                var video = await _videoBLL.GetVideo(id);
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

                var newVideo = await _videoBLL.AddVideo(video);
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

                var checkVideo = await _videoBLL.GetVideo(id);
                if (checkVideo == null)
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }

                return Ok(await _videoBLL.UpdateVideo(video));
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
                var checkVideo = await _videoBLL.GetVideo(id);
                if (checkVideo == null)
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }

                return Ok(await _videoBLL.DeleteVideo(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av video");
            }
        }
    }
}
