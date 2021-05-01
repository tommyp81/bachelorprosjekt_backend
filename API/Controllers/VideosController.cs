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
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideos()
        {
            try
            {
                var videos = await _videoBLL.GetVideos();
                if (videos != null)
                {
                    return Ok(videos);
                }
                else
                {
                    return NotFound($"Ingen videoer ble funnet");
                }
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
                if (video != null)
                {
                    return Ok(video);
                }
                else
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }
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
                if (video != null)
                {
                    var newVideo = await _videoBLL.AddVideo(video);
                    return CreatedAtAction(nameof(GetVideo), new { id = newVideo.Id }, newVideo);
                }
                else
                {
                    return BadRequest("Video objekt mangler");
                }
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
                if (id == video.Id)
                {
                    var updateVideo = await _videoBLL.UpdateVideo(video);
                    if (updateVideo != null)
                    {
                        return Ok(updateVideo);
                    }
                    else
                    {
                        return NotFound($"Video med ID {id} ble ikke funnet");
                    }
                }
                else
                {
                    return BadRequest("Video ID stemmer ikke");
                }
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
                var deleteVideo = await _videoBLL.DeleteVideo(id);
                if (deleteVideo != null)
                {
                    return Ok(deleteVideo);

                }
                else
                {
                    return NotFound($"Video med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av video");
            }
        }
    }
}
