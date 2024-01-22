using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
