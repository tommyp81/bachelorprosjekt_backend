using API.Auth;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[action]")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        // Controller for Custom API Backend

        private readonly ICustomBLL _customBLL;
        private readonly ITokenService _tokenService;

        public CustomController(ICustomBLL customBLL, ITokenService tokenService)
        {
            _customBLL = customBLL;
            _tokenService = tokenService;
        }

        // GET: GetDocuments
        // GET: GetDocuments?infoTopicId=1&pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Id
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDTO>>> GetDocuments(int? infoTopicId, int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste videoer med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Id";

                return Ok(await _customBLL.PagedList(infoTopicId, page, size, order, type));

                // Viser kun dokumenter som har en InfoTopicId
                //var documents = await _customBLL.GetDocuments();
                //if (documents != null)
                //{
                //    return Ok(documents);
                //}
                //else
                //{
                //    return NotFound($"Ingen dokumenter ble funnet");
                //}
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
                if (newDocument != null)
                {
                    return CreatedAtAction(nameof(GetDocumentInfo), new { id = newDocument.Id }, newDocument);
                }
                else
                {
                    return BadRequest("Dokument ble ikke lastet opp");
                }
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
                if (document != null)
                {
                    return Ok(document);
                }
                else
                {
                    return NotFound($"Dokument med ID {id} ble ikke funnet");
                }
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
                var file = await _customBLL.GetDocument(id);
                if (file != null)
                {
                    return File(file.FileStream, file.ContentType, file.FileDownloadName);
                }
                else
                {
                    return NotFound($"Dokumentet med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av dokument");
            }
        }

        // DELETE: DeleteDocument/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DocumentDTO>> DeleteDocument(int id)
        {
            try
            {
                var document = await _customBLL.DeleteDocument(id);
                if (document != null)
                {
                    return Ok(document);
                }
                else
                {
                    return NotFound($"Dokument med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av dokument");
            }
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuthResponse>> Login([FromForm] AuthRequest resquest)//[FromForm] string username, [FromForm] string email, [FromForm] string password
        {
            try
            {
                var response = await _customBLL.Login(resquest);
                if (response != null)
                {
                    // Ok hvis brukernavn/epost og passord stemmer
                    var jwtToken = _tokenService.GenerateJwtToken(response);
                    response.Token = jwtToken;
                    return Ok(response);
                }
                else
                {
                    return Unauthorized("Feil ved brukernavn, epost eller passord");
                }
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
                var user = await _customBLL.SetAdmin(id, admin);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved endring av admin");
            }
        }

        // POST: SetUsername
        [HttpPost]
        public async Task<ActionResult<UserDTO>> SetUsername([FromForm] int id, [FromForm] string username)
        {
            try
            {
                var user = await _customBLL.SetUsername(id, username);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Brukernavn eksisterer allerede");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved endring av brukernavn");
            }
        }

        // GET: SearchDocuments?query=eksempel tekst
        // GET: SearchDocuments?query=eksempel tekst&infoTopicId=1&pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Id
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDTO>>> SearchDocuments(string query, int? infoTopicId, int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste søk i dokumenter med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Id";

                return Ok(await _customBLL.Search(query, infoTopicId, page, size, order, type));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved søk i kommentarer");
            }
        }
    }
}
