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
using System.Reflection;
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
        private IUserEventsModel UserEventsModel;
        private IShopListModel ShopListModel;
        private IServicer Servicer;
        private ISessionManager SessionManager;

        private readonly VerrukkullukContext _context;
        public VerrukkullukController(IVerModel verModel, IHomeModel homeModel, IUserRecipesModel userRecipesModel, IFavoritesModel favoritesModel, IDetailsModel detailsModel, IEventModel eventModel, IUserEventsModel userEventsModel, IShopListModel shopListModel, IServicer servicer, ISessionManager sessionManager, VerrukkullukContext context)
        {
            VerModel = verModel;
            HomeModel = homeModel;
            UserRecipesModel = userRecipesModel;
            FavoritesModel = favoritesModel;
            DetailsModel = detailsModel;
            EventModel = eventModel;
            UserEventsModel = userEventsModel;
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
            var recipe = Servicer.GetRecipeInfoById(Id);
            if (recipe == null)
            {
                return RedirectToAction(nameof(Index));
            }
            DetailsModel.Recipe = recipe;

            ViewData["Title"] = "Recept";
            ViewData["HideCarousel"] = true;
            ViewData["ShowBanner"] = true;
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
        public async Task<IActionResult> MijnFavorieten()
        {
            FavoritesModel.Recipes = await Servicer.GetUserFavorites();
            return View("MyFavorites", FavoritesModel);
        }
        private void FillModel(AddRecipe model)
        {
            model.Products = Servicer.GetAllProducts();
            model.MyKitchenTypeOptions.AddRange(Servicer.GetAllActiveKitchenTypes().Select(kt => new SelectListItem { Value = kt.Id.ToString(), Text = kt.Name }).ToList());
            model.Recipe.Instructions = (model.Recipe.Instructions ?? new string[0]).Append("").ToArray();

            if (model.AddedIngredients != null)
            {
                foreach (Ingredient ingredient in model.AddedIngredients)
                {
                    Product? product = Servicer.GetProductById(ingredient.ProductId);
                    if (product != null)
                    {
                        model.Recipe.Ingredients.Add(new Ingredient(product.Name, ingredient.Amount, product) { Id = ingredient.Id });
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult ReceptMaken()
        {
            AddRecipe model = new AddRecipe();
            FillModel(model);
            return base.View("CreateRecipe", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdateRecipe([FromForm] AddRecipe modifiedRecipe)
        {
            if (modifiedRecipe.DeleteImage && modifiedRecipe.Recipe.ImageObjId > 0)
            {
                if (modifiedRecipe.Recipe.ImageObjId != modifiedRecipe.OriginalImageObjId)
                {
                    Servicer.DeletePicture(modifiedRecipe.Recipe.ImageObjId);
                }
                modifiedRecipe.Recipe.ImageObjId = modifiedRecipe.OriginalImageObjId;
            }
            if (modifiedRecipe.DishPhoto != null)
            {
                if (modifiedRecipe.DishPhoto.Length >= 32 * 1024 * 1024)
                {
                    ModelState.AddModelError(nameof(AddRecipe.DishPhoto), "Plaatje is te groot");
                }
                if (ModelState[nameof(AddRecipe.DishPhoto)]?.ValidationState == ModelValidationState.Valid)
                {
                    // Store the photo
                    modifiedRecipe.Recipe.ImageObjId = await Servicer.SavePictureAsync(modifiedRecipe.DishPhoto);
                }
            }
            else if (modifiedRecipe.Recipe.ImageObjId > 0)
            {
                // Uploaded in previous attempt
                ModelState.Remove(nameof(AddRecipe.DishPhoto));
            }
            //remove empty instruction steps
            modifiedRecipe.Recipe.Instructions = modifiedRecipe.Recipe.Instructions.Where(i => i != null).ToArray();
            if (modifiedRecipe.Recipe.Instructions.Length == 0)
            {
                ModelState.AddModelError(nameof(AddRecipe.Recipe.Instructions), "Voeg tenminste 1 instructie stap toe");
            }

            modifiedRecipe.Recipe.Creator = await VerModel.GetLoggedInUserAsync(User);
            modifiedRecipe.Recipe.CreationDate = DateOnly.FromDateTime(DateTime.Now);

            if (ModelState.IsValid)
            {
                // returned ingredienten overzetten naar Ingredients property
                for (int i = 0; i < modifiedRecipe.AddedIngredients.Length; i++)
                {
                    Ingredient ingredient = modifiedRecipe.AddedIngredients[i];
                    ingredient.Recipe = modifiedRecipe.Recipe;
                    modifiedRecipe.Recipe.Ingredients.Add(ingredient);

                }
                if (modifiedRecipe.Recipe.Id > 0)
                {
                    Servicer.UpdateRecipe(modifiedRecipe.Recipe);
                    if (modifiedRecipe.OriginalImageObjId != modifiedRecipe.Recipe.ImageObjId)
                    {
                        Servicer.DeletePicture(modifiedRecipe.OriginalImageObjId);
                    }
                }
                else
                {
                    Servicer.SaveRecipe(modifiedRecipe.Recipe);
                }
                return RedirectToAction("Recept", new { Id = modifiedRecipe.Recipe.Id });
            }
            FillModel(modifiedRecipe);
            return View("CreateRecipe", modifiedRecipe);
        }

        [HttpGet]
        public IActionResult EditRecipe(int id)
        {
            RecipeInfo? r = Servicer.GetRecipeInfoById(id);
            if (r == null)
            {
                return NotFound();
            }
            AddRecipe model = new AddRecipe(r);
            FillModel(model);
            return base.View("CreateRecipe", model);
        }

        public async Task<IActionResult> ReceptVerwijderen(int id)
        {
            await Servicer.DeleteUserRecipeAsync(id);
            UserRecipesModel.Recipes = Servicer.GetUserRecipes();

            if (VerModel.Error.IsNullOrEmpty())
            {
                return View("MyRecipes", UserRecipesModel);
            }
            else
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
            Recipe? recipe = Servicer.GetRecipeById(recipeId);
            if (recipe == null)
            {
                return Json(new { success = false, message = "Recipe not found" });
            }
            string result = SessionManager.AddRecipeToShoppingList(recipe);
            if (result == "success")
            {
                return Json(new { success = true, message = "Recipe added to shopping list successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to add recipe" });
            }
        }



        //events

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
        public async Task<IActionResult> UserEvents(string? userEmail)
        {
            if (userEmail == null)
            {
                UserEventsModel.SignedUpEvents = await Servicer.GetCurrentUserEvents();
            } else { 
                UserEventsModel.SignedUpEvents = Servicer.GetUserEvents(userEmail);
            }

            return View("UserEvents", UserEventsModel);
        }



        [HttpPost]
        public IActionResult EventSignUp(string name, string email, int eventId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
            {
                var succeed = Servicer.AddParticipantToEvent(name, email, eventId);
                if (!succeed)
                {
                    return NotFound();
                }
                EventModel.Event = Servicer.GetEventById(eventId);

                return View("ThankYou", EventModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please provide valid name and email.");
                return View("EventParticipation");
            }
        }



        [HttpPost]
        public IActionResult EventSignOut(string userEmail, int eventId)
        {
            Servicer.RemoveParticipantFromEvent(userEmail, eventId);

            UserEventsModel.SignedUpEvents = Servicer.GetUserEvents(userEmail);

            return View("UserEvents", UserEventsModel);

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
