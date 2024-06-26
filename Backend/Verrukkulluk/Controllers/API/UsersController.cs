﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IServicer _servicer;

        public UsersController(IMapper mapper, UserManager<User> userManager, IServicer servicer)
        {
            _mapper = mapper;
            _userManager = userManager;
            _servicer = servicer;
        }

        //// GET: api/Users
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<User> users = await _userManager.GetUsersInRoleAsync("VerUser");
            IEnumerable<UserDTO> userDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            IEnumerable<User> admins = await _userManager.GetUsersInRoleAsync("Admin");
            IEnumerable<UserDTO> adminDTO = _mapper.Map<IEnumerable<UserDTO>>(admins);
            foreach (UserDTO admin in adminDTO) { 
                admin.isAdmin = true; 
            };
            return Ok(userDTO.Concat(adminDTO).OrderBy(u => u.FirstName));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userDetails = new UserDetailsModel()
            {
                User = user,
                Recipes = _servicer.GetRecipesByUserId(id).ToArray(),
                RecipeRatings = _servicer.GetRatingsByUserId(id).ToArray()
            };

            var userDetailsDTO = _mapper.Map<UserDetailsDTO>(userDetails);
            
            userDetailsDTO.CommentedRecipe = userDetails.RecipeRatings.GroupBy(ur => ur.Recipe)
                                                        .Select(gr => new RecipeBaseDTO { 
                                                            Id = gr.Key.Id, 
                                                            Title = gr.Key.Title,
                                                            Comments = gr.Select(c => _mapper.Map<CommentDTO>(c)).ToList() 
                                                        }).ToList();

            // Add the own comments to the own recipes
            foreach (RecipeBaseDTO recipe in userDetailsDTO.Recipes)
            {
                recipe.Comments = userDetails.RecipeRatings.Where(r => r.RecipeId == recipe.Id).Select(r => _mapper.Map<CommentDTO>(r)).ToList();
            }
            // Add the own comments to the favourite recipes
            foreach (RecipeBaseDTO recipe in userDetailsDTO.FavouriteRecipes)
            {
                recipe.Comments = userDetails.RecipeRatings.Where(r => r.RecipeId == recipe.Id).Select(r => _mapper.Map<CommentDTO>(r)).ToList();
            }

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

            // Validation?

            //find the user by Id and put data into user object
            User? user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User not found");
            }
            //update user with modified properties from DTO
            user.FirstName = userDTO.FirstName;
            user.CityOfResidence = userDTO.CityOfResidence;
            user.Email = userDTO.Email;
            user.PhoneNumber = userDTO.PhoneNumber;

            //update database with modified user object and store result
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(result);
            }

            // If succeeded, map the updated user back to a DTO and return
            UserDTO updatedUserDTO = _mapper.Map<UserDTO>(user);
            return Ok(updatedUserDTO);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("User does not exist");
            }
            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return UnprocessableEntity(result);
            }
            return NoContent();
        }



    }
}
