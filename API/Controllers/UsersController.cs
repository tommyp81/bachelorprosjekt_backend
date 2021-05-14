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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Controller for Users API Backend

        private readonly IUserBLL _userBLL;

        public UsersController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        // GET: Users
        // GET: Users?pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Id
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste brukere med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Id";

                return Ok(await _userBLL.PagedList(page, size, order, type));

                //var users = await _userBLL.GetUsers();
                //if (users != null)
                //{
                //    return Ok(users);
                //}
                //else
                //{
                //    return NotFound($"Ingen brukere ble funnet");
                //}
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av brukere");
            }
        }

        // GET: Users/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                var user = await _userBLL.GetUser(id);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av bruker");
            }
        }

        // POST: Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser(NewUser user)
        {
            try
            {
                var newUser = await _userBLL.AddUser(user);
                if (newUser != null)
                {
                    return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
                }
                else
                {
                    return BadRequest("Brukernavn eller epost eksisterer allerede");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av bruker");
            }
        }

        // PUT: Users/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, NewUser user)
        {
            try
            {
                if (id == user.Id)
                {
                    var updateUser = await _userBLL.UpdateUser(user);
                    if (updateUser != null)
                    {
                        return Ok(updateUser);
                    }
                    else
                    {
                        return BadRequest("Brukernavn eller epost eksisterer allerede");
                    }
                }
                else
                {
                    return BadRequest("Bruker ID stemmer ikke");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppdatering av bruker");
            }
        }

        // DELETE: Users/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int id)
        {
            try
            {
                var deleteUser = await _userBLL.DeleteUser(id);
                if (deleteUser != null)
                {
                    return Ok(deleteUser);

                }
                else
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av bruker");
            }
        }

        // GET: Users/Search?query=eksempel tekst
        // GET: Users/Search?query=eksempel tekst&?pageNumber=1&pageSize=10&sortOrder=Asc&sortType=Id
        [HttpGet("{Search}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Search(string query, int? pageNumber, int? pageSize, string sortOrder, string sortType)
        {
            try
            {
                // Liste søk i brukere med paging
                var page = pageNumber ?? 1;
                var size = pageSize ?? 10;
                var order = sortOrder ?? "Asc";
                var type = sortType ?? "Id";

                return Ok(await _userBLL.Search(query, page, size, order, type));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved søk i brukere");
            }
        }
    }
}
