using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models.ViewModels;

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

        /// <summary>
        /// Get information of all image Objects (without the binary data)
        /// </summary>
        /// <returns>The list of image info objects </returns>
        // GET: api/<ImgObjController>
        [HttpGet]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of Object info", typeof(List<ImageObjInfo>))]
        public IEnumerable<ImageObjInfo> Get()
        {
            return _crud.ReadAllIPictureIds();
        }

        /// <summary>
        /// Get the complete Image Object (including the binary data)
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The image object</returns>
        // GET api/<ImgObjController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of Object info", typeof(ImageObj))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When the id is not found")]
        public ActionResult<ImageObj> Get(int id)
        {
            ImageObj? imageObj = _crud.ReadImageById(id);
            if (imageObj == null)
            {
                return NotFound();
            }
            return imageObj;
        }

        /// <summary>
        /// Create a new Image Object
        /// </summary>
        /// <param name="image">The image object</param>
        /// <returns>The <code>Id</code> of the created object</returns>
        /// <remarks>
        /// 
        /// Sample request
        ///  
        ///     POST /ImageObj
        ///     {
        ///         "ImageExtention": "png",
        ///         "ImageContent": "iVBORw0KGgoAAAANSUhEUgAAAxgAAAM ... SchXfoPpRz7xZVg1IhySPUNySqqKtbMWAIU1N2TkFotH"
        ///     }
        /// </remarks>
        // POST api/<ImgObjController>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "The id of the created image", typeof(int))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When the request was wrong", typeof(ErrorExample))]
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

        /// <summary>
        /// Modify an existing image object
        /// </summary>
        /// <param name="id">The id of the image object</param>
        /// <param name="image">(body) the image object</param>
        /// <remarks>
        /// 
        /// Sample request
        ///  
        ///     PUT /ImageObj/14
        ///     {
        ///         "id": 14,
        ///         "ImageExtention": "jpeg",
        ///         "ImageContent": "iVBORw0KGgoAAAANSUhEUgAAAxgAAAM ... SchXfoPpRz7xZVg1IhySPUNySqqKtbMWAIU1N2TkFotH"
        ///     }
        /// </remarks>

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "On success")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "When the request was wrong", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When the id is not found")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "On failure")]
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

        /// <summary>
        /// Delete an imageObj (that is no longer referenced)
        /// </summary>
        /// <param name="id">The id of the image object</param>
        // DELETE api/<ImgObjController>/5
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "On success")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "On failure")]
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
