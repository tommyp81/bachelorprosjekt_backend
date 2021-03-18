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
    public class TopicsController : ControllerBase
    {
        // Controller for Topics API Backend

        private readonly ITopicBLL _repository;

        public TopicsController(ITopicBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: Topics
        [HttpGet]
        public async Task<ActionResult<ICollection<TopicDTO>>> GetTopics()
        {
            try
            {
                return Ok(await _repository.GetTopics());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av emne");
            }
        }

        // GET: Topics/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TopicDTO>> GetTopic(int id)
        {
            try
            {
                var topic = await _repository.GetTopic(id);
                if (topic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }
                return Ok(topic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av emne");
            }
        }

        // POST: Topics
        [HttpPost]
        public async Task<ActionResult<TopicDTO>> AddTopic(Topic topic)
        {
            try
            {
                if (topic == null)
                {
                    return BadRequest();
                }
                var newTopic = await _repository.AddTopic(topic);
                return CreatedAtAction(nameof(GetTopic), new { id = newTopic.Id }, newTopic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av nytt emne");
            }
        }

        // PUT: Topics/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TopicDTO>> UpdateTopic(int id, Topic topic)
        {
            try
            {
                if (id != topic.Id)
                {
                    return BadRequest("Emne ID stemmer ikke");
                }
                var updateTopic = await _repository.GetTopic(id);
                if (updateTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }
                return await _repository.UpdateTopic(topic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av emne");
            }
        }

        // DELETE: Topics/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TopicDTO>> DeleteTopic(int id)
        {
            try
            {
                var deleteTopic = await _repository.GetTopic(id);
                if (deleteTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }
                return await _repository.DeleteTopic(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av emne");
            }
        }
    }
}
