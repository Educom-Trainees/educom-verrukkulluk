using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;
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

        //getComments (alle comments ophalen)
        //GetUserComments (alle comments per user ophalen)
        //PutComment (comment van een gebruiker aanpassen)
        //DeleteComment (comment van gebruiker verwijderen)


        //GetAllRatings/getAllComments: alle comments/ratings ophalen: ReadAllRatings
        //GetByUserId: comments per user ophalen:  ReadRatingsByUserId (crud)
        //GetByRecipeId: comments per recept (alle comments van verschillende users): ReadRatingsByRecipeId (deze functie zelf net gemaakt)

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

        //api/Comments/users/id


        //GET: api/Comments/5
        //[HttpGet("{id}")]
        //[Produces("application/json")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetComment(int id)
        //{
        //    //waar haal ik recipeId vandaan? Nu maar even hardcoded.
        //    var recipeId = 2;

        //    CommentDTO commentDTO = _mapper.Map<CommentDTO>(_crud.ReadUserComment(recipeId, id));

        //    if (commentDTO == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(commentDTO);

        //}

        //// PUT: api/Comments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutComment(int id, Comment comment)
        //{
        //    if (id != comment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(comment).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CommentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        //// DELETE: api/Comments/5
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
