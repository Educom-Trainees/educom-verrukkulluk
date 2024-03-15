using Microsoft.AspNetCore.Mvc;
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

        // GET: api/<AllergiesController>
        [HttpGet]
        public IEnumerable<PackagingType> Get()
        {
            return _crud.ReadAllPackagingTypes();
        }
    }
}
