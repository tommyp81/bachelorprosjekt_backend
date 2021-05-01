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
                var subtopic = await _subTopicBLL.GetSubTopic(id);
                if (subtopic == null)
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }

                return Ok(subtopic);
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
                if (subtopic == null)
                {
                    return BadRequest();
                }

                var newSubTopic = await _subTopicBLL.AddSubTopic(subtopic);
                return CreatedAtAction(nameof(GetSubTopic), new { id = newSubTopic.Id }, newSubTopic);
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
                if (id != subtopic.Id)
                {
                    return BadRequest("Underemne ID stemmer ikke");
                }

                var checkSubTopic = await _subTopicBLL.GetSubTopic(id);
                if (checkSubTopic == null)
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }

                return Ok(await _subTopicBLL.UpdateSubTopic(subtopic));
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
                var checkSubTopic = await _subTopicBLL.GetSubTopic(id);
                if (checkSubTopic == null)
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }

                return Ok(await _subTopicBLL.DeleteSubTopic(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av underemne");
            }
        }
    }
}
