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
        public async Task<ActionResult<IEnumerable<InfoTopicDTO>>> GetInfoTopics()
        {
            try
            {
                var infoTopics = await _infoTopicBLL.GetInfoTopics();
                if (infoTopics != null)
                {
                    return Ok(infoTopics);
                }
                else
                {
                    return NotFound($"Ingen emner ble funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av emner");
            }
        }

        // GET: InfoTopics/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InfoTopicDTO>> GetInfoTopic(int id)
        {
            try
            {
                var infoTopic = await _infoTopicBLL.GetInfoTopic(id);
                if (infoTopic != null)
                {
                    return Ok(infoTopic);
                }
                else
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }
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
                if (infotopic != null)
                {
                    // Legg til kommentaren i databasen og fil på Azure Storage og databasen hvis fil er sendt med
                    var newInfoTopic = await _infoTopicBLL.AddInfoTopic(infotopic);
                    return CreatedAtAction(nameof(GetInfoTopic), new { id = newInfoTopic.Id }, newInfoTopic);
                }
                else
                {
                    return BadRequest("Emne objekt mangler");
                }
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
                if (id == infotopic.Id)
                {
                    var updateInfoTopic = await _infoTopicBLL.UpdateInfoTopic(infotopic);
                    if (updateInfoTopic != null)
                    {
                        return Ok(updateInfoTopic);
                    }
                    else
                    {
                        return NotFound($"Emne med ID {id} ble ikke funnet");
                    }
                }
                else
                {
                    return BadRequest("Emne ID stemmer ikke");
                }
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
                var deleteInfoTopic = await _infoTopicBLL.DeleteInfoTopic(id);
                if (deleteInfoTopic != null)
                {
                    return Ok(deleteInfoTopic);
                }
                else
                {
                    return NotFound($"Emne med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av emne");
            }
        }
    }
}
