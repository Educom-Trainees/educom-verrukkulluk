using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System;
using Verrukkulluk.Models;

namespace Verrukkulluk.Controllers
{
    public class ImageController : Controller
    {
        private IServicer Servicer;

        public ImageController(IServicer servicer)
        {
            Servicer = servicer;
        }

        public IActionResult GetImage(int id)
        {
            var imageObj = Servicer.GetImage(id);
            byte[] image = imageObj.ImageContent;
            string extension = imageObj.ImageExtention;

            return File(image, $"image/{extension}");
        }
    }
}