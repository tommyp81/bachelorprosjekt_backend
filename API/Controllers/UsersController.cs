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
                var users = await _userBLL.GetUsers();
                if (users != null)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound($"Ingen brukere ble funnet");
                }
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
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser(AuthUser user)
        {
            try
            {
                if (user != null)
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
                else
                {
                    return BadRequest("Bruker objekt mangler");
                }
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
    }
}
