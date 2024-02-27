using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Formats.Asn1;
using Verrukkulluk.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Verrukkulluk.Data;
using Verrukkulluk.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Verrukkulluk;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        private IVerModel VerModel;
        private IHomeModel HomeModel;
        private IUserRecipesModel UserRecipesModel;
        private IFavoritesModel FavoritesModel;
        private IDetailsModel DetailsModel;
        private IEventModel EventModel;
        private IShopListModel ShopListModel;
        private IServicer Servicer;
        private ISessionManager SessionManager;

        private readonly VerrukkullukContext _context;
        public VerrukkullukController(IVerModel verModel, IHomeModel homeModel, IUserRecipesModel userRecipesModel, IFavoritesModel favoritesModel, IDetailsModel detailsModel, IEventModel eventModel, IShopListModel shopListModel, IServicer servicer, ISessionManager sessionManager, VerrukkullukContext context)
        {
            VerModel = verModel;
            HomeModel = homeModel;
            UserRecipesModel = userRecipesModel;
            FavoritesModel = favoritesModel;
            DetailsModel = detailsModel;
            EventModel = eventModel;
            ShopListModel = shopListModel;
            Servicer = servicer;
            SessionManager = sessionManager;
            _context = context;
        }
        public IActionResult Index()
        {
            HomeModel.Recipes = Servicer.GetAllRecipes();
            HomeModel.Events = Servicer.GetAllEvents();
            System.Console.WriteLine(HomeModel.Recipes.Count);
            return View(HomeModel);
        }
        public IActionResult Recept(int Id = 1)
        {
            DetailsModel.Recipe = Servicer.GetRecipeById(Id);

            ViewData["Title"]= "Recept";
            ViewData["HideCarousel"]= true;
            ViewData["ShowBanner"]= true;
            ViewData["PictureLocation"] = "/images/pexels-ella-olsson.jpg";
            return View("Recipe", DetailsModel);
        }

        [Authorize(Roles = "VerUser, Admin")]
        public IActionResult MijnRecepten()
        {
            UserRecipesModel.Recipes = Servicer.GetUserRecipes();
            return View("MyRecipes", UserRecipesModel);
        }

        [Authorize(Roles = "VerUser, Admin")]
        public IActionResult MijnFavorieten()
        {
            FavoritesModel.Recipes = Servicer.GetUserFavorites();
            return View("MyFavorites", FavoritesModel);
        }
        private async Task FillModel(AddRecipe model)
        {
            model.Products = Servicer.GetAllProducts();
            model.MyKitchenTypeOptions.AddRange(await _context.KitchenTypes.Select(kt => new SelectListItem { Value = kt.Id.ToString(), Text = kt.Name }).ToListAsync());

            if (model.AddedIngredients != null) {
                foreach(Ingredient ingredient in model.AddedIngredients) {
                    Product? product = Servicer.GetProductById(ingredient.ProductId);
                    if (product != null) {
                        model.Ingredients.Add(new Ingredient(product.Name, ingredient.Amount, product));
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReceptMaken()
        {
            ViewData["Title"] = "Recept Maken";
            AddRecipe model = new AddRecipe();
            model.Instructions = new string[] { "" };
            await FillModel(model);
            return base.View("CreateRecipe", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptMaken([FromForm]AddRecipe recipe)
        {
            if (recipe.ImageObjId > 0 && recipe.DeleteImage) {
                // TODO Servicer.DeletePicture(recipe.ImageObjId);
                recipe.ImageObjId = 0;
            }
            if (recipe.DishPhoto != null) {
                if (ModelState[nameof(AddRecipe.DishPhoto)]?.ValidationState == ModelValidationState.Valid) { 
                    // Store the photo
                    recipe.ImageObjId = await Servicer.SavePictureAsync(recipe.DishPhoto);
                }
            }
            else if (recipe.ImageObjId > 0)
            {
                // Uploaded in previous attempt
                ModelState.Remove(nameof(AddRecipe.DishPhoto));
            }

            // Modelstate verwijderen voor de objecten
            ModelState.Remove(nameof(AddRecipe.Creator));
            ModelState.Remove(nameof(AddRecipe.KitchenType));
            for (int i = 0; i < recipe.AddedIngredients.Length; i++)
            {
                string key = $"AddedIngredients[{i}].";
                ModelState.Remove(key + "Product");
                ModelState.Remove(key + "Recipe");
            }

            recipe.Creator = await VerModel.GetLoggedInUserAsync(User);
            recipe.CreationDate = DateOnly.FromDateTime(DateTime.Now);


            if (ModelState.IsValid)
            {
                // bijbehorende ingredienten invullen
                for (int i = 0; i < recipe.AddedIngredients.Length; i++)
                {
                    Ingredient ingredient = recipe.AddedIngredients[i];
                    ingredient.Recipe = recipe;
                    recipe.Ingredients.Add(ingredient);

                }
                Servicer.SaveRecipe(recipe);
                return RedirectToAction("Recept", new { Id = recipe.Id });
            }
            await FillModel(recipe);
            return View("CreateRecipe", recipe);
        }

        public IActionResult ReceptVerwijderen(int id)
        {
            Servicer.DeleteUserRecipe(id);
            UserRecipesModel.Recipes = Servicer.GetUserRecipes();

            if (VerModel.Error.IsNullOrEmpty())
            {
                return View("MyRecipes", UserRecipesModel);
            } else
            {
                return View("MyRecipes", UserRecipesModel);
            }
        }

        public IActionResult MijnBoodschappenlijst()
        {
            ShopListModel.ShopList = SessionManager.GetShoppingList();
            return View("Shoplist", ShopListModel);
        }

        [HttpPost]
        public IActionResult RateRecipe(int recipeId, int ratingValue, string comment)
        {
            bool success = Servicer.RateRecipe(recipeId, ratingValue, comment);
            return Json(new { success });
        }

        [HttpGet]
        public IActionResult GetUserRating(int recipeId)
        {
            int? userRating = Servicer.GetUserRating(recipeId);
            string? comment = Servicer.GetUserComment(recipeId);
            return Json(new { rating = userRating, comment = comment });
        }

        [HttpPost]
        public IActionResult UpdateAverageRating(int recipeId)
        {
            try
            {
                Servicer.UpdateAverageRating(recipeId);
                return Ok("Average rating updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the average rating: " + ex.Message);
            }
        }

        public IActionResult Event(int id)
        {
            EventModel.Event = Servicer.GetEventById(id);
            return View(EventModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(VerModel model)
        {
            VerModel.Input = model.Input;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                SignInResult result = await VerModel.Login(VerModel.Input);
                if (result.Succeeded)
                {
                    HomeModel.Recipes = Servicer.GetAllRecipes();
                    return RedirectToAction(nameof(Index), HomeModel);
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(nameof(VerModel.Input) + "." + nameof(VerModel.Input.Email), "Invalid login attempt.");
                    HomeModel.Recipes = Servicer.GetAllRecipes();
                    return View(nameof(Index), HomeModel);
                }
            }
            // If we got this far, something failed, redisplay form
            HomeModel.Recipes = Servicer.GetAllRecipes();
            return View(nameof(Index), HomeModel);
        }
        public IActionResult AddToShoppingList(CartItem newItem)
        {
            var shoppingList = SessionManager.GetShoppingList();
            shoppingList.Add(newItem);
            SessionManager.SaveShoppingList(shoppingList);
            return RedirectToAction("MijnBoodschappenlijst");
        }

        [HttpGet]
        [Route("RemoveAllShopItems")]
        public IActionResult RemoveAllItems()
        {
            SessionManager.SaveShoppingList(new List<CartItem>());
            return Json(new { success = true });
        }
        [HttpPost]
        [Route("RemoveShopItemByName")]
        public IActionResult RemoveItemByName(string itemName)
        {
            var shoppingList = SessionManager.GetShoppingList();
            var removedCount = shoppingList.RemoveAll(item => item.Name == itemName);

            if (removedCount > 0)
            {
                SessionManager.SaveShoppingList(shoppingList);
                return Json(new { success = true, removedCount });
            }

            return Json(new { success = false, message = "Item not found" });
        }

        public IActionResult AddRecipeToShoppingList(int recipeId)
        {
            Recipe Recipe = Servicer.GetRecipeById(recipeId);
            string result = SessionManager.AddRecipeToShoppingList(Recipe);
            if (result == "success") { 
                return Json(new { success = true, message = "Recipe added to shopping list successfully" }); 
            } else
            {
                return Json(new { success = false, message = "Recipe not found" });
            }
        }
    }
}
