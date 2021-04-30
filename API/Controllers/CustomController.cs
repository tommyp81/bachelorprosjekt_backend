using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model.Domain_models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        // Controller for Custom API Backend

        private readonly ICustomBLL _customBLL;
        private readonly IConfiguration _config;
        private readonly IUserBLL _userBLL;

        public CustomController(ICustomBLL customBLL, IConfiguration configuration, IUserBLL userBLL)
        {
            _customBLL = customBLL;
            _config = configuration;
            _userBLL = userBLL;
        }

        // GET: GetDocuments
        [HttpGet]
        public async Task<ActionResult<ICollection<DocumentDTO>>> GetDocuments()
        {
            try
            {
                // Viser kun dokumenter som har en InfoTopicId
                return Ok(await _customBLL.GetDocuments());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av dokumenter");
            }
        }

        // POST: UploadDocument
        [HttpPost]
        public async Task<ActionResult<DocumentDTO>> UploadDocument(
            [FromForm] IFormFile file, [FromForm] int? userId, [FromForm] int? postId, [FromForm] int? commentId, [FromForm] int? infoTopicId)
        {
            try
            {
                // Sjekk at nødvendig data er lagt til
                if (file == null)
                {
                    return BadRequest("Det må legges ved en fil");
                }
                if (userId == null)
                {
                    return BadRequest("Det må legges ved en bruker ID");
                }
                if (postId == null && commentId == null && infoTopicId == null)
                {
                    return BadRequest("Det må legges ved enten post ID, kommentar ID eller InfoTopic ID");
                }
                if (postId != null && commentId != null && infoTopicId == null)
                {
                    return BadRequest("Det kan ikke legges ved både post ID og kommentar ID");
                }
                if (postId != null && commentId != null && infoTopicId != null)
                {
                    return BadRequest("Det kan ikke legges ved tre IDer (post ID, kommentar ID og InfoTopic ID)");
                }

                // Legg til fil på Azure Storage og i databasen
                var newDocument = await _customBLL.UploadDocument(file, userId, postId, commentId, infoTopicId);
                return CreatedAtAction(nameof(GetDocumentInfo), new { id = newDocument.Id }, newDocument);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved opplasting av nytt dokument");
            }
        }

        // GET: GetDocumentInfo/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DocumentDTO>> GetDocumentInfo(int id)
        {
            try
            {
                var document = await _customBLL.GetDocumentInfo(id);
                if (document == null)
                {
                    return NotFound($"Dokument med ID {id} ble ikke funnet");
                }

                return Ok(document);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av dokument");
            }
        }

        // GET: GetDocument/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            try
            {
                var document = await _customBLL.GetDocumentInfo(id);
                if (document == null)
                {
                    return NotFound($"Dokumentet med ID {id} finnes ikke i databasen");
                }

                var file = await _customBLL.GetDocument(id);
                if (file == null)
                {
                    return NotFound($"Dokumentet med ID {id} finnes ikke i Azure Storage");
                }

                return File(file.FileStream, file.ContentType, file.FileDownloadName);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);//"Feil ved henting av dokument"
            }
        }

        // DELETE: DeleteDocument/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DocumentDTO>> DeleteDocument(int id)
        {
            try
            {
                var document = await _customBLL.GetDocumentInfo(id);
                if (document == null)
                {
                    return NotFound($"Dokument med ID {id} ble ikke funnet");
                }

                return Ok(await _customBLL.DeleteDocument(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av dokument");
            }
        }

        // POST: Login
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login([FromForm] string username, [FromForm] string email, [FromForm] string password)
        {
            try
            {
                var user = await _customBLL.Login(username, email, password);
                if (user != null)
                {
                    // Ok hvis brukernavn/epost og passord stemmer
                    return Ok(user);
                }

                return Unauthorized("Feil ved brukernavn, epost eller passord");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved login");
            }
        }

        // POST: SetAdmin
        [HttpPost]
        public async Task<ActionResult<UserDTO>> SetAdmin([FromForm] int id, [FromForm] bool admin)
        {
            try
            {
                var user = await _userBLL.GetUser(id);
                if (user == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }

                return Ok(await _customBLL.SetAdmin(id, admin));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved endring av admin");
            }
        }
    }
}
