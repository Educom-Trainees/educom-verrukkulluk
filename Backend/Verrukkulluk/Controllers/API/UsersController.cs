using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models.ViewModels;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICrud _crud;
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IServicer _servicer;

        public UsersController(ICrud crud, IMapper mapper, UserManager<User> userManager, IServicer servicer)
        {
            _crud = crud;
            _mapper = mapper;
            this._userManager = userManager;
            _servicer = servicer;
        }

        //// GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> users = await _userManager.GetUsersInRoleAsync("VerUser");
            users = users.Concat(await _userManager.GetUsersInRoleAsync("Admin"));

            IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(userDTO);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDetailsDTO> GetUserById(int id)
        {

            User user = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            UserDetailsModel userDetails = new UserDetailsModel()
            {
                User = user,
                Recipes = _servicer.GetRecipesByUserId(id).ToArray(),
                RecipeRatings = _servicer.GetRatingsByUserId(id).ToArray()
            };

            UserDetailsDTO userDetailsDTO = _mapper.Map<UserDetailsDTO>(userDetails);

            return Ok(userDetailsDTO);
        }



        //PUT: api/Users
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UserDTO>> PutUser(int id, UserDTO userDTO)
        {

            if (id != userDTO.Id)
            {
                return BadRequest("Ids must match");
            }

            User user = _mapper.Map<User>(userDTO);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(result);
            }

            return Ok(result);

        }

        ///DeleteById ()

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
