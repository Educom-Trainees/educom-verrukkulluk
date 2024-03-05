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

        public List<RecipeInfo> GetRecipesByUserId(int userId) {
                return Crud.ReadAllRecipesByUserId(userId) ?? new List<RecipeInfo>();
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

        //public List<RecipeInfo> GetFavoritesByUserId(int userId) {
            //return Crud.ReadAllRecipesByUserId(userId) ?? new List<RecipeInfo>();
        //}

        public List<Product> GetAllProducts()
        {
            return Crud.ReadAllProducts();
        }


        public void DeleteUserRecipe(int recipeId)
        {
            var user = UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User).Result;

            if (UserManager.IsInRoleAsync(user, "Admin").Result)
            {
                // If the user is an admin, directly delete the recipe
                string result = Crud.DeleteRecipe(recipeId);

                if (result != "Recept verwijderd.")
                {
                    throw new Exception(result);
                }
            }
            else
            {
                // If the user is not an admin, proceed with checking user ID and deleting user recipe
                string tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
                int userId = int.Parse(tempUserId);

                string result = Crud.DeleteUserRecipe(userId, recipeId);

                if (result != "Recept verwijderd.")
                {
                    throw new Exception(result);
                }
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

        public bool RateRecipe(int recipeId, int ratingValue, string comment)
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
            return Crud.AddOrUpdateRecipeRating(recipeId, parsedUserId, ratingValue, comment);
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

        public List<RecipeRating> GetRatingsByUserId(int userId) {
            return Crud.ReadRatingsByUserId(userId);
        }

        public string? GetUserComment(int recipeId)
        {
            var userId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            int parsedUserId;
            if (userId == null || !int.TryParse(userId, out parsedUserId))
            {
                return null;
                
            }
            return Crud.ReadUserComment(recipeId, parsedUserId);
        }

        public void UpdateAverageRating(int recipeId)
        {
            Crud.UpdateAverageRating(recipeId);
        }
        public async Task<int> SavePictureAsync(IFormFile picture)
        {
            using (var memoryStream = new MemoryStream())
            {
                await picture.CopyToAsync(memoryStream);
                var imageObj = new ImageObj(memoryStream.ToArray(), Path.GetExtension(picture.FileName));
                Crud.CreatePicture(imageObj);
                return imageObj.Id;
            }
        }

        public void SaveRecipe(Recipe recipe)
        {
            Crud.CreateRecipe(recipe);
        }

        public void UpdateRecipe(Recipe recipe) {
            Crud.UpdateRecipe(recipe);
        }

        public void DeletePicture(int id) {
            Crud.DeletePicture(id);
        }
    }
}
