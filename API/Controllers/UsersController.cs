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
    public class UsersController : ControllerBase
    {
        // Controller for Users API Backend

        private readonly IUserBLL _userBLL;

        public UsersController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                return Ok(await _userBLL.GetUsers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av bruker");
            }
        }

        // GET: Users/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                var user = await _userBLL.GetUser(id);
                if (user == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved henting av bruker");
            }
        }

        // POST: Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser(AuthUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var newUser = await _userBLL.AddUser(user);
                if (newUser == null)
                {
                    return BadRequest("Brukernavn eller epost eksisterer allerede");
                }

                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny bruker");
            }
        }

        // PUT: Users/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, AuthUser user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest("Bruker ID stemmer ikke");
                }

                var checkUser = await _userBLL.GetUser(id);
                if (checkUser == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }

                var updateUser = await _userBLL.UpdateUser(user);
                if (updateUser == null)
                {
                    return BadRequest("Brukernavn eller epost eksisterer allerede");
                }

                return Ok(updateUser);
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
                var checkUser = await _userBLL.GetUser(id);
                if (checkUser == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }

                return Ok(await _userBLL.DeleteUser(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av bruker");
            }
        }
    }
}
