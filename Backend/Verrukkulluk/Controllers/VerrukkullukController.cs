﻿using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        public IActionResult Recept()
        {
            return View("Recipe");
        }
    }
}
