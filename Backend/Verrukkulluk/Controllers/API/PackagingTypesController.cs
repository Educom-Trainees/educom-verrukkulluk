using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOmodels;
using Verrukkulluk.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagingTypesController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly IMapper _mapper;
        private readonly ILogger<ImageObjController> _logger;

        public PackagingTypesController(ICrud crud, IMapper mapper, ILogger<ImageObjController> logger)
        {
            _crud = crud;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/PackagingTypes
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "On success", typeof(IEnumerable<KitchenTypeDTO>))]
        public IEnumerable<PackagingTypeDTO> Get()
        {
            // return all read packaging type from the crud as DTO objects
            return _crud.ReadAllPackagingTypes().Select(_mapper.Map<PackagingTypeDTO>);
        }

        // POST api/PackagingTypes
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "When successfully created", typeof(KitchenTypeDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When an field is incorrect", typeof(ErrorExample))]

        public ActionResult<PackagingTypeDTO> Post([FromBody] PackagingTypeDTO packagingType)
        {
            ValidatePackagingType(packagingType);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newPackagingType = _mapper.Map<PackagingType>(packagingType);
            _crud.CreatePackagingType(newPackagingType);
            return Ok(_mapper.Map<PackagingTypeDTO>(packagingType));
        }

        // PUT api/PackagingTypes/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When an field is incorrect", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When a packaging type is not found")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "When unable to process")]
        public ActionResult Put(int id, [FromBody] PackagingTypeDTO packagingType)
        {
            ValidatePackagingType(packagingType, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _crud.UpdatePackagingType(_mapper.Map<PackagingType>(packagingType));
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

        private void ValidatePackagingType(PackagingTypeDTO packagingType, int id = 0)
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
        /// <summary>
        ///     This method allows you to toggle the active status of a packaging type.
        /// </summary>
        /// <remarks>
        /// Sample request
        ///  
        ///     PATCH /PackagingTypes/4/active                   - toggles the active flag 
        ///     
        ///     PATCH /PackagingTypes/4/active?active=false      - disables the active flag
        /// 
        /// </remarks>
        [HttpPatch("{id}/active")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When packaging type not found")]
        public IActionResult ToggleActive(int id, [FromQuery]bool? active)
        {
            var packagingType = _crud.ReadPackagingTypeById(id);
            if (packagingType == null)
            {
                return NotFound();
            }
            
            packagingType.Active = active ?? !packagingType.Active;
            _crud.UpdatePackagingType(packagingType);
        
            return NoContent();
        }

        /// <summary>
        ///     This method allows you to delete a packaging type when not in use or not the packaging type "Overig".
        /// </summary>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When packaging type not found")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "When packaging type still in use")]
        public IActionResult Delete(int id)
        {
            var packagingType = _crud.ReadPackagingTypeById(id);
            if (packagingType == null)
            {
                return NotFound();
            }
            if (_crud.IsPackagingTypeUsed(id))
            {
                return Problem("packagingType is still in use, try to deactivate it", statusCode: 422);
            }
        
            _crud.DeletePackagingType(packagingType);
        
            return NoContent();
        }
    }
}
