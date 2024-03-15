using Microsoft.AspNetCore.Mvc;
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
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update image {image.Id} failed", image.Id);
                return Problem(statusCode: 500);
            }
            return NoContent();
        }

        // DELETE api/<ImgObjController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _crud.DeletePicture(id);
        }
    }
}
