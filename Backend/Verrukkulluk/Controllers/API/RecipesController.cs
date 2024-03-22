using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Swashbuckle.AspNetCore.Annotations;
using Verrukkulluk;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models.ViewModels;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(ICrud crud, IMapper mapper, ILogger<RecipesController> logger)
        {
            _crud = crud;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns>The list of recipes</returns>
        //// GET: api/Recipes
        [HttpGet]
        [Produces("application/json")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<RecipeDTO>))]
        public ActionResult<IEnumerable<RecipeDTO>> GetRecipes()
        {
            var recipes = _crud.ReadAllRecipes();
            return Ok(_mapper.Map<IEnumerable<RecipeDTO>>(recipes) ?? Enumerable.Empty<RecipeDTO>());
        }

        /// <summary>
        /// Get a specific Recipe
        /// </summary>
        /// <param name="id">The id of the recipe</param>
        /// <returns>The Recipe</returns>
        // GET: api/Recipes/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RecipeDTO> GetRecipe(int id)
        {
            var recipe = _crud.ReadRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return _mapper.Map<RecipeDTO>(recipe);
        }

        /// <summary>
        /// Update an recipe
        /// </summary>
        /// <param name="id">The id of the recipe</param>
        /// <param name="recipeDTO">The updated recipe</param>
        /// <remarks>
        /// Save (when the image has changed) the image first using a POST on /api/ImageObj, use the resulting number (for example 61) as image object id
        /// 
        /// Sample request
        ///  
        ///     PUT /Recipes/6
        ///     {
        ///       "id": 6,
        ///       "title": "Salade",
        ///       "description": "Een lekkere gerecht, snel klaar te maken en een favoriet van het hele gezin.",
        ///       "kitchenTypeId": 7,
        ///       "instructions": [
        ///          "Doe boter in de pan.",
        ///          "Bak de hamburger.",
        ///          "Snij sla, tomaten en een bolletje.",
        ///          "Doe de hamburger tussen bolletje met de sla en tomaten."
        ///       ],
        ///       "creationDate": "2024-03-22",
        ///       "creatorId": 1,
        ///       "numberOfPeople": 4,
        ///       "imageObjId": 61,
        ///       "ingredients": [
        ///          {
        ///            "id": 14,
        ///            "name": "tomaten",
        ///            "amount": 1,
        ///            "productId": 5
        ///          }
        ///       ]
        ///     }
        /// 
        /// </remarks>
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When there is a problem", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [HttpPut("{id}")]
        public IActionResult PutRecipe(int id, RecipeDTO recipeDTO)
        {
            ValidateRecipe(recipeDTO, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var recipe = _mapper.Map<Recipe>(recipeDTO);
                int previousImageObjId = _crud.ReadImageObjIdForRecipeId(recipeDTO.Id);

                _crud.UpdateRecipe(recipe);
                if (previousImageObjId != recipe.ImageObjId)
                {
                    _crud.DeletePicture(previousImageObjId);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && _crud.ReadRecipeById(id) == null)
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update recipe {recipeDTO.Title} failed", recipeDTO.Title);
                return UnprocessableEntity();
            }
        }



        /// <summary>
        /// Create a new Recipe
        /// </summary>
        /// <param name="recipeDTO">The recipe</param>
        /// <returns>The created Recipe</returns>
        /// <remarks>
        /// Save the image first using a POST on /api/ImageObj, use the resulting number (for example 61) as image object id
        /// 
        /// Sample request
        ///  
        ///     POST /Recipes
        ///     {
        ///         "id": 2,
        ///         "title": "Duitse Hamburger",
        ///         "description": "Een lekkere gerecht, snel klaar te maken en een favoriet van het hele gezin.",
        ///         "kitchenTypeId": 12,
        ///         "kitchenTypeName": "Overig",
        ///         "comments": [],
        ///         "instructions": [
        ///           "Doe boter in de pan.",
        ///           "Bak het ei."
        ///         ],
        ///         "creatorId": 1,
        ///         "numberOfPeople": 4,
        ///         "imageObjId": 61
        ///         "ingredients" : [
        ///           { 
        ///             "name": "eieren",
        ///             "productId": 3,
        ///             "amount": 1
        ///           }
        ///         ]
        ///     }
        /// </remarks>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RecipeDTO> PostRecipe(RecipeDTO recipeDTO)
        {
            ValidateRecipe(recipeDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var recipe = _mapper.Map<Recipe>(recipeDTO);
            try
            {
                _crud.CreateRecipe(recipe);
                recipe.KitchenType = _crud.ReadKitchenTypeById(recipe.KitchenTypeId)!;
                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    ingredient.Product = _crud.ReadProductById(ingredient.ProductId)!;
                }
                return _mapper.Map<RecipeDTO>(recipe);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "failed to create recipe");
                return Problem(statusCode: 500);
            }
        }

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
        private void ValidateRecipe(RecipeDTO recipe, int id = 0)
        {
            if (recipe.Id != id)
            {
                ModelState.AddModelError(nameof(RecipeDTO.Id), $"Id must be identical to {id}");
            }
            if (_crud.DoesRecipeTitleAlreadyExist(recipe.Title, recipe.Id))
            {
                ModelState.AddModelError(nameof(RecipeDTO.Title), "There is another allergy with this name");
            }
            if (!_crud.DoesKitchenTypeExist(recipe.KitchenTypeId))
            {
                ModelState.AddModelError(nameof(RecipeDTO.KitchenTypeId), "The kitchenType is not known");
            }
            if (!_crud.DoesPictureExist(recipe.ImageObjId))
            {
                ModelState.AddModelError(nameof(RecipeDTO.ImageObjId), "The image is not (yet) stored");
            }
            else if (!_crud.IsPictureAvailiable(recipe.ImageObjId, EImageObjType.Recipe, recipe.Id))
            {
                ModelState.AddModelError(nameof(RecipeDTO.ImageObjId), "The image is used by another object");
            }
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                IngredientDTO ingredient = recipe.Ingredients[i];
                if (ingredient.Amount < 0.25)
                {
                    ModelState.AddModelError(nameof(RecipeDTO.Ingredients) + $"[{i}]." + nameof(IngredientDTO.Amount), "Amount must be >= 0.25");
                }
                if (!_crud.DoesProductIdExists(ingredient.ProductId))
                {
                    ModelState.AddModelError(nameof(RecipeDTO.Ingredients) + $"[{i}]." + nameof(IngredientDTO.ProductId), $"Unknown productId {ingredient.ProductId}");
                }
            }
        }
    }
}
