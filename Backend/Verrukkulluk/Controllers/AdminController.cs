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
            //favorites to be implemented
            var detailModel = new UserDetailsModel() {
                User = user,
            };
            FillUserDetails(id, detailModel);
            return View(detailModel);
        }

        private void FillUserDetails(int id, UserDetailsModel detailModel) {
            detailModel.Recipes = _servicer.GetRecipesByUserId(id).ToArray();
            detailModel.RecipeRatings = _servicer.GetRatingsByUserId(id).ToArray();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(int id, UserDetailsModel model) {
            User? user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) {
                return NotFound();
            }
            User? other = await _userManager.FindByEmailAsync(user?.Email ?? "");
            if (other?.Id != id)
            {
                ModelState.AddModelError("User.Email", "Email already bound to other account");
            }
            user.FirstName = model.User.FirstName;
            user.PhoneNumber = model.User.PhoneNumber;
            user.CityOfResidence = model.User.CityOfResidence;
            user.Email = model.User.Email;
            //user.PasswordHash = model.User.PasswordHash;
            if (ModelState.IsValid) {
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Details", new { id = id });
            }
            FillUserDetails(id, model);
            return View("Details", model);
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

        public IActionResult Recipe(int id) {
            RecipeInfo r = _servicer.GetRecipeInfoById(id);
            if (r == null) {
                return NotFound();
            }
            return View(r);
        }

        [HttpPost]
        public IActionResult RemoveRecipe(int recipeId)
        {
            var recipe = _servicer.GetRecipeInfoById(recipeId);
            if (recipe != null)
            {
                _servicer.DeleteUserRecipe(recipeId);
            }
            return RedirectToAction("Index");
        }
    }
}
