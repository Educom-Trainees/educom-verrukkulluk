using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Verrukkulluk.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergiesController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly ILogger<ImageObjController> _logger;

        public AllergiesController(ICrud crud, ILogger<ImageObjController> logger)
        {
            _crud = crud;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve all active allergies
        /// </summary>
        /// <returns>A list of allergies</returns>
        /// <response code="200">A list of allergies</response>
        // GET: api/allergies
        [HttpGet]
        [Produces("application/json")]
        [SwaggerResponse(200, Type=typeof(List<Allergy>))]
        public IEnumerable<Allergy> Get()
        {
            return _crud.ReadAllAllergies();
        }

        /// <summary>
        /// Get a specific Allergy
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Allergy</returns>
        // GET api/allergies/5
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "The allergy with this id", typeof(Allergy))]
        [SwaggerResponse(404, "When not found")]
        [HttpGet("{id}")]
        public ActionResult<Allergy> Get(int id)
        {
            Allergy? allergy = _crud.ReadAllergyById(id);
            if (allergy == null)
            {
                return NotFound();
            }
            return allergy;
        }

        /// <summary>
        /// Create a new Allergy
        /// </summary>
        /// <param name="allergy">The allergy</param>
        /// <returns>The created Allergy</returns>
        /// <remarks>
        /// Save the image first using a POST on /api/ImageObj, use the resulting number (for example 56) as image object id
        /// 
        /// Sample request
        ///  
        ///     POST /Allergies
        ///     {
        ///         "name": "Nuts",
        ///         "imgObjId": 56
        ///     }
        /// </remarks>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "The created allergy", typeof(Allergy))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When there is a problem", typeof(ErrorExample))]
        public ActionResult<Allergy> Post([FromBody] Allergy allergy)
        {
            ValidateAllergy(allergy);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _crud.CreateAllergy(allergy);
            return Ok(allergy);
        }

        /// <summary>
        /// Modifies an existing Allergy
        /// </summary>
        /// <param name="id">The id of the allergy</param>
        /// <param name="allergy">The modified allergy</param>
        /// <remarks>
        /// To change the image, save the image first using a POST on /api/ImageObj, use the resulting number (for example 57) as image object id
        /// 
        /// Sample request
        ///  
        ///     PUT /Allergies/14
        ///     {
        ///         "id": 14,
        ///         "name": "Nuts",
        ///         "imgObjId": 57
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When there is a problem", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Put(int id, [FromBody] Allergy allergy)
        {
            ValidateAllergy(allergy, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _crud.UpdateAllergy(allergy);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && !_crud.DoAllergiesExist([id]))
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update allergy {allergy.Name} failed", allergy.Name);
                return UnprocessableEntity();
            }
        }

        private void ValidateAllergy(Allergy allergy, int id = 0)
        {
            if (allergy.Id != id)
            {
                ModelState.AddModelError(nameof(Allergy.Id), $"Id must be identical to {id}");
            }
            if (_crud.DoesAllergyNameAlreadyExist(allergy.Name, allergy.Id))
            {
                ModelState.AddModelError(nameof(Allergy.Name), "There is another allergy with this name");
            }
            if (!_crud.DoesPictureExist(allergy.ImgObjId))
            {
                ModelState.AddModelError(nameof(Allergy.ImgObjId), "The image is not (yet) stored");
            } else if (!_crud.IsPictureAvailiable(allergy.ImgObjId, EImageObjType.Allergy, allergy.Id))
            {
                ModelState.AddModelError(nameof(Allergy.ImgObjId), "The image is used by another object");
            }
        }
    }

   /*public class AllergyExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Allergy
            {
                Id = 2,
                Name = "Nuts",
                ImgObjId = 57
            };
        }
    }
   */
}
