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
            return View(VerModel);
        }
        public IActionResult Recept()
        {
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
                case "test1":
                    return new Event
                    {
                        Title = "Test1",
                        Description = "Test1 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht",
                        Price = 12.99m
                    };
                case "test2":
                    return new Event
                    {
                        Title = "Test2",
                        Description = "Test2 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht",
                        Price = 12.99m
                    };
                case "test3":
                    return new Event
                    {
                        Title = "Test3",
                        Description = "Test3 description",
                        Date = new DateOnly(2024, 01, 30),
                        StartTime = new TimeOnly(14, 0),
                        EndTime = new TimeOnly(16, 0),
                        Place = "Jaarbeurs Utrecht",
                        Price = 12.99m
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
