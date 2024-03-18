using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageObjController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly ILogger<ImageObjController> _logger;

        public ImageObjController(ICrud crud, ILogger<ImageObjController> logger)
        {
            _crud = crud;
            _logger = logger;
        }

        // GET: api/<ImgObjController>
        [HttpGet]
        public IEnumerable<ImageObjInfo> Get()
        {
            return _crud.ReadAllIPictureIds();
        }

        // GET api/<ImgObjController>/5
        [HttpGet("{id}")]
        public ActionResult<ImageObj> Get(int id)
        {
            ImageObj? imageObj = _crud.ReadImageById(id);
            if (imageObj == null)
            {
                return NotFound();
            }
            return imageObj;
        }

        // POST api/<ImgObjController>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post([FromBody] ImageObj image)
        {
            if (image.Id != 0)
            {
                ModelState.AddModelError(nameof(ProductDTO.Id), "Id must be 0");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _crud.CreatePicture(image);
            return Ok(image.Id);
        }

        // PUT api/<ImgObjController>/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Put(int id, [FromBody] ImageObj image)
        {
            if (image.Id != id)
            {
                ModelState.AddModelError(nameof(ProductDTO.Id), $"Id must be identical to {id}");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _crud.UpdatePicture(image);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && !_crud.DoesPictureExist(id))
                {
                    return NotFound();
                }

                _logger.LogError(e, "Update image {image.Id} failed", image.Id);
                return UnprocessableEntity();
            }
        }

        // DELETE api/<ImgObjController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Delete(int id)
        {
            try
            {
                _crud.DeletePicture(id);
                return NoContent();
            } 
            catch
            {
                return UnprocessableEntity("Failed to delete image, maybe in use?");
            }

        }
    }
}
