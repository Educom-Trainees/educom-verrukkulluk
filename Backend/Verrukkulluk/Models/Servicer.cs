using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Verrukkulluk.Models
{
    public class Servicer: IServicer
    {
        private readonly ICrud Crud;
        private readonly UserManager<User> UserManager;
        private readonly IHttpContextAccessor HttpContextAccessor;
        public readonly SignInManager<User> SignInManager;
        public Servicer()
        {

        }

        public Servicer(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            Crud = crud;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
            SignInManager = signInManager;
        }

        public List<RecipeInfo> GetAllRecipes()
        {
            return Crud.ReadAllRecipes();
        }

        public List<RecipeInfo> GetUserRecipes()
        {
            if (HttpContextAccessor?.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }

            var tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            if (int.TryParse(tempUserId, out int userId))
            {
                return Crud.ReadAllRecipesByUserId(userId);
            }
            else
            {
                return null;
            }
        }

        public List<RecipeInfo> GetUserFavorites()
        {
            if (HttpContextAccessor?.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }
            var tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            if (int.TryParse(tempUserId, out int userId))
            {
                //Hoe de favorieten uit de crud gehaald moeten worden. !Nog aanpassen! Nu zijn het de recepten die iemand zelf gemaakt heeft
                return Crud.ReadAllRecipesByUserId(userId);
            }
            return null;
        }

        public List<Product> GetAllProducts()
        {
            return Crud.ReadAllProducts();
        }


        public void DeleteUserRecipe(int recipeId)
        {
            string tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            int userId = int.Parse(tempUserId);

            //UserId is supplied so that only the recipe's creator can delete their recipe
            string result = Crud.DeleteUserRecipe(userId, recipeId);

            if (result != "Recept verwijderd.")
            {
                throw new Exception(result);
            }
        }

        public async Task<SignInResult> Login(InputModel input)
        {
            return await SignInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
        }

        public Product? GetProductById(int productId)
        {
            return Crud.ReadProductById(productId);
        }

        public RecipeInfo GetRecipeById(int Id)
        {
            return Crud.ReadRecipeById(Id);
            //base64RecipePicture = Convert.ToBase64String(Recipe.DishPhoto);
        }

        public ImageObj GetImage(int Id)
        {
            return Crud.ReadImageById(Id);
        }

        public Event GetEventById(int Id)
        {
            return Crud.ReadEventById(Id);
        }

        public List<Event> GetAllEvents()
        {
            return Crud.ReadAllEvents();
        }

        public bool RateRecipe(int recipeId, int ratingValue)
        {
            var userId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            int? parsedUserId =null;
            if (userId != null && int.TryParse(userId, out int userIdValue))
            {
                parsedUserId = userIdValue;
            }
            else
            {
                var sessionRatings = HttpContextAccessor.HttpContext.Session.Get<Dictionary<int, int>>("SessionRatings") ?? new Dictionary<int, int>();
                sessionRatings[recipeId] = ratingValue;
                HttpContextAccessor.HttpContext.Session.Set("SessionRatings", sessionRatings);
            }
            return Crud.AddRecipeRating(recipeId, parsedUserId, ratingValue);
        }

        public int? GetUserRating(int recipeId)
        {
            var userId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            int parsedUserId;
            if (userId == null || !int.TryParse(userId, out parsedUserId))
            {
                var sessionRatings = HttpContextAccessor.HttpContext.Session.Get<Dictionary<int, int>>("SessionRatings");
                if (sessionRatings != null && sessionRatings.ContainsKey(recipeId))
                {
                    return -sessionRatings[recipeId];
                }
                else
                {
                    return null;
                }
            }
            return Crud.ReadUserRating(recipeId, parsedUserId);
        }
    }
}
