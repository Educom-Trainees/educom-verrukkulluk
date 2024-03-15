using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Data;

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

        // GET: api/<AllergiesController>
        [HttpGet]
        public IEnumerable<Allergy> Get()
        {
            return _crud.ReadAllAllergies();
        }
    }
}
