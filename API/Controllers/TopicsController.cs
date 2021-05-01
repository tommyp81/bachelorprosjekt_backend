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

        private readonly ITopicBLL _topicBLL;

        public TopicsController(ITopicBLL topicBLL)
        {
            _topicBLL = topicBLL;
        }

        // GET: Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetTopics()
        {
            try
            {
                return Ok(await _topicBLL.GetTopics());
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
                var topic = await _topicBLL.GetTopic(id);
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

                var newTopic = await _topicBLL.AddTopic(topic);
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

                var checkTopic = await _topicBLL.GetTopic(id);
                if (checkTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }

                return Ok(await _topicBLL.UpdateTopic(topic));
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
                var checkTopic = await _topicBLL.GetTopic(id);
                if (checkTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }

                return Ok(await _topicBLL.DeleteTopic(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av emne");
            }
        }
    }
}
