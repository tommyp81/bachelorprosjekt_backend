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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class SubTopicsController : ControllerBase
    {
        // Controller for SubTopics API Backend

        private readonly ISubTopicBLL _subTopicBLL;

        public SubTopicsController(ISubTopicBLL subTopicBLL)
        {
            _subTopicBLL = subTopicBLL;
        }

        // GET: SubTopics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubTopicDTO>>> GetSubTopics()
        {
            try
            {
                return Ok(await _subTopicBLL.GetSubTopics());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av underemne");
            }
        }

        // GET: SubTopics/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubTopicDTO>> GetSubTopic(int id)
        {
            try
            {
                var subTopic = await _subTopicBLL.GetSubTopic(id);
                if (subTopic != null)
                {
                    return Ok(subTopic);
                }
                else
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av underemne");
            }
        }

        // POST: SubTopics
        [HttpPost]
        public async Task<ActionResult<SubTopicDTO>> AddSubTopic(SubTopic subtopic)
        {
            try
            {
                if (!(string.IsNullOrEmpty(subtopic.Title) || string.IsNullOrEmpty(subtopic.Description)))
                {
                    var newSubTopic = await _subTopicBLL.AddSubTopic(subtopic);
                    return CreatedAtAction(nameof(GetSubTopic), new { id = newSubTopic.Id }, newSubTopic);

                }
                else
                {
                    return BadRequest("Underemne mangler");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av nytt underemne");
            }
        }

        // PUT: SubTopics/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubTopicDTO>> UpdateSubTopic(int id, SubTopic subtopic)
        {
            try
            {
                if (id == subtopic.Id)
                {
                    var updateSubTopic = await _subTopicBLL.UpdateSubTopic(subtopic);
                    if (updateSubTopic != null)
                    {
                        return Ok(updateSubTopic);
                    }
                    else
                    {
                        return NotFound($"Underemne med ID {id} ble ikke funnet");
                    }
                }
                else
                {
                    return BadRequest("Underemne ID stemmer ikke");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av underemne");
            }
        }

        // DELETE: SubTopics/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SubTopicDTO>> DeleteSubTopic(int id)
        {
            try
            {
                var deleteSubTopic = await _subTopicBLL.DeleteSubTopic(id);
                if (deleteSubTopic != null)
                {
                    return Ok(deleteSubTopic);

                }
                else
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av underemne");
            }
        }
    }
}
