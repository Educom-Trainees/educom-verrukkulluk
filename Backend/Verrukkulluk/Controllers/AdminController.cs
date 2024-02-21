using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.ViewModels;

namespace Verrukkulluk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IServicer _servicer;

        public AdminController(UserManager<User> userManager, IServicer servicer)
        {
            _userManager = userManager;
            _servicer = servicer;
        }

        public IActionResult Index()
        {
            var model = new AdminModel
            {
                Users = _userManager.Users.ToList(),
                Recipes = _servicer.GetAllRecipes()
            };

            return View("Admin", model);
        }

        [HttpPost]
        public IActionResult RemoveUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveRecipe(int recipeId)
        {
            var recipe = _servicer.GetRecipeById(recipeId);
            if (recipe != null)
            {
                _servicer.DeleteUserRecipe(recipeId);
            }
            return RedirectToAction("Index");
        }
    }
}
