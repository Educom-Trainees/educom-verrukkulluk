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
using Verrukkulluk.Models.DbModels;
using Microsoft.Extensions.Logging;


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
        public VerrukkullukController(IVerModel verModel, IHomeModel homeModel, IUserRecipesModel userRecipesModel, IFavoritesModel favoritesModel, IDetailsModel detailsModel, IEventModel eventModel, IShopListModel shopListModel, IServicer servicer, ISessionManager sessionManager)
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
        }
        public IActionResult Index()
        {
            HomeModel.Recipes = Servicer.GetAllRecipes();
            HomeModel.Events = Servicer.GetAllEvents();
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
        private void FillModel(AddRecipe model)
        {
            model.Products = Servicer.GetAllProducts();

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
        public IActionResult ReceptMaken()
        {
            ViewData["Title"] = "Recept Maken";
            AddRecipe model = new AddRecipe();
            model.Instructions = new string[] { "" };
            FillModel(model);
            return base.View("CreateRecipe", model);
        }

        [HttpPost]
        public IActionResult ReceptMaken(AddRecipe recipe)
        {
                System.Console.WriteLine(recipe.Instructions.Length);
            if (ModelState.IsValid)
            {
                //Recept opslaan en weergeven van detailpagina met het toegevoegde recept
                // recipe.DishPhoto apart opslaan en id toevoegen aan recipe
                //!Nog aanpassen! Nu tijdelijk:
                return RedirectToAction("Index");
            }
            FillModel(recipe);
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

        public IActionResult JoinEvent(int id)
        {
            EventModel.Event = Servicer.GetEventById(id);

            return View("EventParticipation", EventModel);
        }


        [HttpPost]
        public IActionResult EventSignUp(string name, string email, int EventId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                
                EventModel.Event = Servicer.AddParticipantToEvent(name, email, EventId);

                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.EventId = EventId;

                return View("ThankYou", EventModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please provide valid name and email.");
                return View("EventParticipation");
            }
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
