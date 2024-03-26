using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenTypesController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly ILogger<KitchenTypesController> _logger;

        public KitchenTypesController(ICrud crud, ILogger<KitchenTypesController> logger)
        {
            _crud = crud;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of kitchen types, sorted by name with the option "Overig" as last item
        /// </summary>
        /// <returns>The sorted list</returns>
        // GET: api/KitchenTypes
        [HttpGet]
        public IEnumerable<KitchenType> Get()
        {
            return _crud.ReadAllKitchenTypes();
        }

        // POST api/KitchenTypes
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] KitchenType kitchenType)
        {
            ValidateKitchenType(kitchenType);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _crud.CreateKitchenType(kitchenType);
            return Ok(kitchenType);
        }

        // PUT api/KitchenTypes/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Put(int id, [FromBody] KitchenType kitchenType)
        {
            ValidateKitchenType(kitchenType, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _crud.UpdateKitchenType(kitchenType);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && !_crud.DoesKitchenTypeExist(id))
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update packagingType {kitchenType.Name} failed", kitchenType.Name);
                return UnprocessableEntity();
            }
        }

        private void ValidateKitchenType(KitchenType kitchenType, int id = 0)
        {
            if (kitchenType.Id != id)
            {
                ModelState.AddModelError(nameof(KitchenType.Id), $"Id must be identical to {id}");
            }
            if (_crud.DoesKitchenTypeNameAlreadyExist(kitchenType.Name, kitchenType.Id))
            {
                ModelState.AddModelError(nameof(Allergy.Name), "There is another kitchen type with this name");
            }
        }
    }
}
