﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ICrud _crud;
        private IMapper _mapper;

        public RecipesController(ICrud crud, IMapper mapper)
        {
            _crud = crud;
            _mapper = mapper;
        }

        //// GET: api/Recipes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        //{
        //    return await _context.Recipes.ToListAsync();
        //}

        //// GET: api/Recipes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Recipe>> GetRecipe(int id)
        //{
        //    var recipe = await _context.Recipes.FindAsync(id);

        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    return recipe;
        //}

        //// PUT: api/Recipes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        //{
        //    if (id != recipe.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(recipe).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RecipeExists(id))
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

        //// POST: api/Recipes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        //{
        //    _context.Recipes.Add(recipe);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        //}

        //// DELETE: api/Recipes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRecipe(int id)
        //{
        //    var recipe = await _context.Recipes.FindAsync(id);
        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Recipes.Remove(recipe);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool RecipeExists(int id)
        //{
        //    return _context.Recipes.Any(e => e.Id == id);
        //}
    }
}