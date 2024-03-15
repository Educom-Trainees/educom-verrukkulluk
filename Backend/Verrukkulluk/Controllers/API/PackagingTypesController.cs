using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagingTypesController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly ILogger<ImageObjController> _logger;

        public PackagingTypesController(ICrud crud, ILogger<ImageObjController> logger)
        {
            _crud = crud;
            _logger = logger;
        }

        // GET: api/PackagingTypes
        [HttpGet]
        public IEnumerable<PackagingType> Get()
        {
            return _crud.ReadAllPackagingTypes();
        }

        // POST api/PackagingTypes
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] PackagingType packagingType)
        {
            ValidatePackagingType(packagingType);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _crud.CreatePackagingType(packagingType);
            return Ok(packagingType);
        }

        // PUT api/PackagingTypes/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Put(int id, [FromBody] PackagingType packagingType)
        {
            ValidatePackagingType(packagingType, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _crud.UpdatePackagingType(packagingType);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && !_crud.DoesPackagingTypeExist(id))
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update packagingType {packagingType.Name} failed", packagingType.Name);
                return UnprocessableEntity();
            }
        }

        private void ValidatePackagingType(PackagingType packagingType, int id = 0)
        {
            if (packagingType.Id != id)
            {
                ModelState.AddModelError(nameof(Allergy.Id), $"Id must be identical to {id}");
            }
            if (_crud.DoesPackagingTypeNameAlreadyExist(packagingType.Name, packagingType.Id))
            {
                ModelState.AddModelError(nameof(Allergy.Name), "There is another packaging type with this name");
            }
        }
    }
}
