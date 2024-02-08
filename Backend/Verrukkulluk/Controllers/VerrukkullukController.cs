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

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        private IVerModel VerModel;
        private IHomeModel HomeModel;
        private IUserRecipesModel UserRecipesModel;
        private IFavoritesModel FavoritesModel;
        private IDetailsModel DetailsModel;
        private IServicer Servicer;
        public VerrukkullukController(IVerModel verModel, IHomeModel homeModel, IUserRecipesModel userRecipesModel, IFavoritesModel favoritesModel, IDetailsModel detailsModel, IServicer servicer)
        {
            VerModel = verModel;
            HomeModel = homeModel;
            UserRecipesModel = userRecipesModel;
            FavoritesModel = favoritesModel;
            DetailsModel = detailsModel;
            Servicer = servicer;
        }
        public IActionResult Index()
        {
            HomeModel.Recipes = Servicer.GetAllRecipes();
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

        [Authorize(Roles = "VerUser")]
        public IActionResult MijnRecepten()
        {
            UserRecipesModel.Recipes = Servicer.GetUserRecipes();
            return View("MyRecipes", UserRecipesModel);
        }

        [Authorize(Roles = "VerUser")]
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
            FillModel(model);
            return base.View("CreateRecipe", model);
        }

        [HttpPost]
        public IActionResult ReceptMaken(AddRecipe recipe)
        {
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
            var shoppingList = GetShoppingList();
            return View("Shoplist", shoppingList);
        }

        public IActionResult Event(string eventName)
        {
            Event eventModel = GetEventData(eventName);
            return View(eventModel);
        }
        //ga database gebruiken en pas toe in view
        private Event GetEventData(string eventName)
        {
            switch (eventName?.ToLower())
            {
                default:
                    return new Event
                    {
                        Title = "Vegetarisch koken",
                        Description = "Een workshop vegetarisch koken, onder leiding van Trientje Hupsakee",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht",
                        Price = 12.99m
                    };
                case "event2":
                    return new Event
                    {
                        Title = "Tafeldekken",
                        Description = "Een workshop om op een snelle en chique manier een dinertafel te dekken",
                        Date = new DateOnly(2024, 02, 15),
                        StartTime = new TimeOnly(12, 0),
                        EndTime = new TimeOnly(17, 0),
                        Place = "De Kuip",
                        Price = 10.49m
                    };
                case "event3":
                    return new Event
                    {
                        Title = "Secuur afwassen",
                        Description = "Hier leert u hoe u kunt afwassen op een veilige en duurzame manier",
                        Date = new DateOnly(2024, 02, 25),
                        StartTime = new TimeOnly(09, 30),
                        EndTime = new TimeOnly(12, 30),
                        Place = "Johan Cruijff ArenA",
                        Price = 15.99m
                    };
                case "event4":
                    return new Event
                    {
                        Title = "Wokken",
                        Description = "Wat is wokken precies en wat maakt het nou zo lekker?",
                        Date = new DateOnly(2024, 03, 04),
                        StartTime = new TimeOnly(10, 0),
                        EndTime = new TimeOnly(12, 30),
                        Place = "Philips Stadion",
                        Price = 18.99m
                    };
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

        private List<CartItem> GetShoppingList()
        {
            var shoppingList = HttpContext.Session.Get<List<CartItem>>("ShoppingList") ?? new List<CartItem>();
            return shoppingList;
        }

        private void SaveShoppingList(List<CartItem> shoppingList)
        {
            HttpContext.Session.Set("ShoppingList", shoppingList);
        }
        public IActionResult AddToShoppingList(CartItem newItem)
        {
            var shoppingList = GetShoppingList();
            shoppingList.Add(newItem);
            SaveShoppingList(shoppingList);
            return RedirectToAction("MijnBoodschappenlijst");
        }

        [HttpGet]
        [Route("fillcart")]
        // A test shopping list with a few items that get stored in the session
        // public IActionResult TestShoppingList()
        // {
        //     var sampleShoppingList = new List<CartItem>
        //     {
        //         new CartItem
        //         {
        //             ImageUrl = "../images/avocado.jpg",
        //             Name = "Avocado",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 4,
        //             Price = 5.99m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/witte_bol.jpg",
        //             Name = "Witte bol",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 1,
        //             Price = 3.49m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/avocado.jpg",
        //             Name = "Avocado2",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 4,
        //             Price = 5.99m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/witte_bol.jpg",
        //             Name = "Witte bol2",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 1,
        //             Price = 3.49m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/witte_bol.jpg",
        //             Name = "Witte bol3",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 1,
        //             Price = 3.49m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/avocado.jpg",
        //             Name = "Avocado3",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 4,
        //             Price = 5.99m
        //         },
        //         new CartItem
        //         {
        //             ImageUrl = "../images/avocado.jpg",
        //             Name = "Avocado4",
        //             Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt, consectetur adipisicing elit sum laude, ut labore et dolore magna aliqua.",
        //             Quantity = 4,
        //             Price = 5.99m
        //         },
        //     };
        //     SaveShoppingList(sampleShoppingList);
        //     return RedirectToAction("MijnBoodschappenlijst");
        // }

        [HttpGet]
        [Route("RemoveAllShopItems")]
        public IActionResult RemoveAllItems()
        {
            SaveShoppingList(new List<CartItem>());
            return Json(new { success = true });
        }
        [HttpPost]
        [Route("RemoveShopItemByName")]
        public IActionResult RemoveItemByName(string itemName)
        {
            var shoppingList = GetShoppingList();
            var removedCount = shoppingList.RemoveAll(item => item.Name == itemName);

            if (removedCount > 0)
            {
                SaveShoppingList(shoppingList);
                return Json(new { success = true, removedCount });
            }

            return Json(new { success = false, message = "Item not found" });
        }

        public IActionResult AddRecipeToShoppingList(int recipeId)
        {
            DetailsModel.Recipe = Servicer.GetRecipeById(recipeId);

            if (DetailsModel.Recipe == null)
            {
                return Json(new { success = false, message = "Recipe not found" });
            }

            var existingShoppingList = HttpContext.Session.Get<List<CartItem>>("ShoppingList");
            var shoppingList = existingShoppingList ?? new List<CartItem>();

            foreach (var ingredient in DetailsModel.Recipe.Ingredients)
            {
                var quantityNeeded = ingredient.Amount;
                var newItem = new CartItem
                {
                    ImageObjId = ingredient.Product.ImageObjId, //Ik weet niet hoe dit meegegeven wordt
                    Name = ingredient.Product.Name,
                    Description = ingredient.Product.Description,
                    Quantity = Math.Round(quantityNeeded, 2),
                    Price = ingredient.Product.Price
                };
                shoppingList.Add(newItem);
            }
            HttpContext.Session.Set("ShoppingList", shoppingList);
            return Json(new { success = true, message = "Recipe added to shopping list successfully" });
        }
    }
}
