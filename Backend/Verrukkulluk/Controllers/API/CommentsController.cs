using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;
using Verrukkulluk.Migrations;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICrud _crud;
        private IMapper _mapper;

        public CommentsController(ICrud crud, IMapper mapper)
        {
            _crud = crud;
            _mapper = mapper;
        }


       //GET: api/Comments
       [HttpGet]
       [Produces("application/json")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<CommentDTO> GetAllComments()
        {
            IEnumerable<RecipeRating> recipeRatings = _crud.ReadAllRatings();
            IEnumerable<CommentDTO> commentDTO = _mapper.Map<IEnumerable<CommentDTO>>(recipeRatings);

            if (commentDTO == null)
            {
                return Enumerable.Empty<CommentDTO>();
            }

            return commentDTO;
        }


        //GET: api/Comments/users/{userId}
        [HttpGet("users/{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCommentsByUserId(int userId)
        {
             IEnumerable<CommentDTO> commentDTO = _mapper.Map<IEnumerable<CommentDTO>>(_crud.ReadRatingsByUserId(userId));

            if (commentDTO == null)
            {
                return NotFound();
            }

            return Ok(commentDTO);

        }


        //GET: api/Comments/recipes/{recipeId}
        [HttpGet("recipes/{recipeId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCommentByRecipeId(int recipeId)
        {

            IEnumerable<CommentDTO> commentDTO = _mapper.Map<IEnumerable<CommentDTO>>(_crud.ReadRatingsByRecipeId(recipeId));

            if (commentDTO == null)
            {
                return NotFound();
            }

            return Ok(commentDTO);

        }


        // PUT: api/Comments/users/{userId}
        [HttpPut("users/{userId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PutComment(int userId, CommentDTO commentDTO)
        {
            if (userId != commentDTO.UserId)
            {
                return BadRequest("Ids must match");
            }

            //find the comment by recipeId and by userId. and put data into RecipeRating object
            RecipeRating? recipeRating =  _crud.ReadRatingByUserIdAndRecipeId(commentDTO.RecipeId, userId);
            if (recipeRating == null)
            {
                return NotFound("Comment not found");
            }

            //update RecipeRating object with modified comment & ratingvalue (from CommentDTO)
            recipeRating.Comment = commentDTO.Comment;
            recipeRating.RatingValue = commentDTO.RatingValue;


            //update database with modified comment object and store result
            bool success = _crud.AddOrUpdateRecipeRating(recipeRating.RecipeId, recipeRating.UserId, recipeRating.RatingValue, recipeRating.Comment);

            if(!success)
            {
                return UnprocessableEntity("Could not update comment");
            }

            // Map the updated RecipeRating object back to a CommentDTO
            CommentDTO updatedCommentDTO = _mapper.Map<CommentDTO>(recipeRating);

            return Ok(updatedCommentDTO);
        }



        // DELETE: api/Comments/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteComment(int id)
        //{
        //    var comment = await _context.Comment.FindAsync(id);
        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Comment.Remove(comment);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CommentExists(int id)
        //{
        //    return _context.Comment.Any(e => e.Id == id);
        //}
    }
}
