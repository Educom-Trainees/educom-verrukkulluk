using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/allergies
        [HttpGet]
        public IEnumerable<Allergy> Get()
        {
            return _crud.ReadAllAllergies();
        }

        // GET api/allergies/5
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

        // POST api/allergies
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] Allergy allergy)
        {
            ValidateAllergy(allergy);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _crud.CreateAllergy(allergy);
            return Ok(allergy);
        }

        // PUT api/allergies/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
}
