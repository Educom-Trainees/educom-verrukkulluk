using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.ViewModels;
using Verrukkulluk.Models.ViewModels;

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

        public IActionResult Details(int id) {
            User user = _userManager.Users.First(u => u.Id == id);
            var recipes = _servicer.GetRecipesByUserId(id);
            var comments = _servicer.GetRatingsByUserId(id);
            //favorites to be implemented
            var detailModel = new UserDetailsModel() {
                User = user,
                Recipes = recipes,
                RecipeRatings = comments
            };
            return View(detailModel);
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
