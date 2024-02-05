using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Verrukkulluk.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Verrukkulluk.Controllers
{
    public class VerrukkullukController : Controller
    {
        private IVerModel VerModel;
        public VerrukkullukController(IVerModel verModel)
        {
            VerModel = verModel;
        }
        public IActionResult Index()
        {
            VerModel.GetAllRecipes();
            return View(VerModel);
        }
        public IActionResult Recept(int Id = 1)
        {
            VerModel.GetCalories(Id);
            VerModel.GetPrice(Id);
            VerModel.GetRecipeById(Id);
            ViewData["Title"]= "Recept";
            ViewData["HideCarousel"]= true;
            ViewData["ShowBanner"]= true;
            ViewData["PictureLocation"] = "/images/pexels-ella-olsson.jpg";
            return View("Recipe", VerModel);
        }

        [Authorize(Roles = "VerUser")]
        public IActionResult MijnRecepten()
        {
            VerModel.GetUserRecipes();
            return View("MyRecipes", VerModel);
        }

        [Authorize(Roles = "VerUser")]
        public IActionResult MijnFavorieten()
        {
            VerModel.GetUserFavorites();
            return View("MyFavorites", VerModel);
        }
        private void FillModel(AddRecipe model)
        {
            model.Products = VerModel.GetAllProducts();

            if (model.AddedIngredients != null) {
                foreach(Ingredient ingredient in model.AddedIngredients) {
                    Product? product = VerModel.GetProductById(ingredient.ProductId);
                    if (product != null) {
                        model.Ingredients.Add(new Ingredient(product.Name, ingredient.Amount, product));
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult ReceptMaken()
        {
            System.Console.WriteLine("get");
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
                //!Nog aanpassen! Nu tijdelijk:
                return RedirectToAction("Index");
            }
            FillModel(recipe);
            System.Console.WriteLine(recipe.AddedIngredients.Count());
            return View("CreateRecipe", recipe);
        }

        public IActionResult ReceptVerwijderen(int id)
        {
            VerModel.DeleteUserRecipe(id);
            VerModel.GetUserRecipes();

            if (VerModel.Error.IsNullOrEmpty())
            {
                return View("MyRecipes", VerModel);
            } else
            {
                return View("MyRecipes", VerModel);
            }
        }

        public IActionResult MijnBoodschappenlijst()
        {
            return View("Shoplist", VerModel);
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
                    return RedirectToAction(nameof(Index), VerModel);
                }
                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(nameof(VerModel.Input) + "." + nameof(VerModel.Input.Email), "Invalid login attempt.");
                    return View(nameof(Index), VerModel);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(nameof(Index), VerModel);
        }

    }
}
