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
            var photo = Servicer.GetImage(id);

            ImageObj imageObj = Servicer.GetImage(id);
            // Extract byte array.
            byte[] image = imageObj.ImageContent;

            // Return byte array as jpeg.
            switch (imageObj.ImageExtention) {
                case "jpeg":
                    return File(image, "image/jpeg");
                case "jpg":
                    return File(image, "image/jpg");
                case "png":
                    return File(image, "image/png");
            }
            return null;
        }
    }
}