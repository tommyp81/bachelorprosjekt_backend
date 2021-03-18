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

        private readonly ISubTopicBLL _repository;

        public SubTopicsController(ISubTopicBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: SubTopics
        [HttpGet]
        public async Task<ActionResult<ICollection<SubTopicDTO>>> GetSubTopics()
        {
            try
            {
                return Ok(await _repository.GetSubTopics());
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
                var subtopic = await _repository.GetSubTopic(id);
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
                var newSubTopic = await _repository.AddSubTopic(subtopic);
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
                var updateSubTopic = await _repository.GetSubTopic(id);
                if (updateSubTopic == null)
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }
                return await _repository.UpdateSubTopic(subtopic);
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
                var deleteSubTopic = await _repository.GetSubTopic(id);
                if (deleteSubTopic == null)
                {
                    return NotFound($"Underemne med ID {id} ble ikke funnet");
                }
                return await _repository.DeleteSubTopic(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av underemne");
            }
        }
    }
}
