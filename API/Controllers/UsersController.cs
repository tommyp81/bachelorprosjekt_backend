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

        private readonly IUserBLL _repository;

        public UsersController(IUserBLL _repository)
        {
            this._repository = _repository;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<ICollection<UserDTO>>> GetUsers()
        {
            try
            {
                return Ok(await _repository.GetUsers());
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
                var user = await _repository.GetUser(id);
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
        public async Task<ActionResult<UserDTO>> CreateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                var newUser = await _repository.AddUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved oppretting av ny bruker");
            }
        }

        // PUT: Users/1
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest("Bruker ID stemmer ikke");
                }
                var updateUser = await _repository.GetUser(id);
                if (updateUser == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }
                return await _repository.UpdateUser(user);
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
                var deleteUser = await _repository.GetUser(id);
                if (deleteUser == null)
                {
                    return NotFound($"Bruker med ID {id} ble ikke funnet");
                }
                return await _repository.DeleteUser(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Feil ved sletting av bruker");
            }
        }
    }
}
