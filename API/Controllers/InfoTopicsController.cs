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
    public class InfoTopicsController : ControllerBase
    {
        // Controller for InfoTopics API Backend

        private readonly IInfoTopicBLL _infoTopicBLL;

        public InfoTopicsController(IInfoTopicBLL infoTopicBLL)
        {
            _infoTopicBLL = infoTopicBLL;
        }

        // GET: InfoTopics
        [HttpGet]
        public async Task<ActionResult<ICollection<InfoTopicDTO>>> GetInfoTopics()
        {
            try
            {
                return Ok(await _infoTopicBLL.GetInfoTopics());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av emne");
            }
        }

        // GET: InfoTopics/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InfoTopicDTO>> GetInfoTopic(int id)
        {
            try
            {
                var infotopic = await _infoTopicBLL.GetInfoTopic(id);
                if (infotopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }

                return Ok(infotopic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av emne");
            }
        }

        // POST: InfoTopics
        [HttpPost]
        public async Task<ActionResult<InfoTopicDTO>> AddInfoTopic(InfoTopic infotopic)
        {
            try
            {
                if (infotopic == null)
                {
                    return BadRequest();
                }

                var newInfoTopic = await _infoTopicBLL.AddInfoTopic(infotopic);
                return CreatedAtAction(nameof(GetInfoTopic), new { id = newInfoTopic.Id }, newInfoTopic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av nytt emne");
            }
        }

        // PUT: InfoTopics/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<InfoTopicDTO>> UpdateInfoTopic(int id, InfoTopic infotopic)
        {
            try
            {
                if (id != infotopic.Id)
                {
                    return BadRequest("Emne ID stemmer ikke");
                }

                var checkInfoTopic = await _infoTopicBLL.GetInfoTopic(id);
                if (checkInfoTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }

                return Ok(await _infoTopicBLL.UpdateInfoTopic(infotopic));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av emne");
            }
        }

        // DELETE: InfoTopics/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<InfoTopicDTO>> DeleteInfoTopic(int id)
        {
            try
            {
                var checkInfoTopic = await _infoTopicBLL.GetInfoTopic(id);
                if (checkInfoTopic == null)
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }

                return Ok(await _infoTopicBLL.DeleteInfoTopic(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av emne");
            }
        }
    }
}
