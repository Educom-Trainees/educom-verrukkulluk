using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DbModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Verrukkulluk.Models
{
    public class Servicer : IServicer
    {
        private readonly ICrud Crud;
        private readonly UserManager<User> UserManager;
        private readonly IHttpContextAccessor HttpContextAccessor;
        public readonly SignInManager<User> SignInManager;
        private readonly ISessionManager sessionManager;

        public Servicer(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, ISessionManager sessionManager)
        {
            Crud = crud;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
            SignInManager = signInManager;
            this.sessionManager = sessionManager;
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
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User.Id");
            }
        }

        public List<RecipeInfo> GetRecipesByUserId(int userId)
        {
            return Crud.ReadAllRecipesByUserId(userId) ?? new List<RecipeInfo>();
        }

        public async Task<List<RecipeInfo>> GetUserFavorites()
        {
            User? user = await GetCurrentUser();
            return user.FavouritesList?.Select(r => new RecipeInfo(r)).ToList() ?? new List<RecipeInfo>();
        }


        public List<Product> GetAllProducts()
        {
            return Crud.ReadAllProducts();
        }


        public async Task<bool> DeleteUserRecipeAsync(int recipeId)
        {
            var user = await GetCurrentUser();

            if (await UserManager.IsInRoleAsync(user, "Admin"))
            {
                // If the user is an admin, directly delete the recipe
                return Crud.DeleteRecipe(recipeId);
            }
            else
            {
                // If the user is not an admin, proceed with checking user ID and deleting user recipe
                return Crud.DeleteUserRecipe(user.Id, recipeId);
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

        public Recipe? GetRecipeById(int id)
        {
            return Crud.ReadRecipeById(id);
        }
        public RecipeInfo? GetRecipeInfoById(int id)
        {
            return Crud.ReadRecipeInfoById(id);
            //base64RecipePicture = Convert.ToBase64String(Recipe.DishPhoto);
        }

        public ImageObj? GetImage(int id)
        {
            return Crud.ReadImageById(id);
        }

        public Event GetEventById(int id)
        {
            return Crud.ReadEventById(id);
        }

        public List<Event> GetAllEvents()
        {
            return Crud.ReadAllEvents();
        }

        public bool RateRecipe(int recipeId, int ratingValue, string comment)
        {
            var parsedUserId = GetCurrentUserId();
            if (parsedUserId == null)
            {
                sessionManager.AddRecipeRating(recipeId, ratingValue);
            }
            return Crud.AddOrUpdateRecipeRating(recipeId, parsedUserId, ratingValue, comment);
        }

        public int? GetUserRating(int recipeId)
        {
            int? parsedUserId = GetCurrentUserId();
            if (parsedUserId == null)
            {
                int? sessionResult = sessionManager.GetRecipeRating(recipeId);
                return sessionResult.HasValue ? -sessionResult.Value : null;
            }
            return Crud.ReadRatingByUserIdAndRecipeId(recipeId, parsedUserId.Value)?.RatingValue;
        }

        public List<RecipeRating> GetRatingsByUserId(int userId)
        {
            return Crud.ReadRatingsByUserId(userId);
        }

        public string? GetUserComment(int recipeId)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return null;

            }
            return Crud.ReadRatingByUserIdAndRecipeId(recipeId, userId.Value)?.Comment;
        }

        public void UpdateAverageRating(int recipeId)
        {
            Crud.UpdateAverageRating(recipeId);
        }
        public async Task<int> SavePictureAsync(IFormFile picture)
        {
#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var memoryStream = new MemoryStream())
            {
                await picture.CopyToAsync(memoryStream);
                var imageObj = new ImageObj(memoryStream.ToArray(), Path.GetExtension(picture.FileName));
                Crud.CreatePicture(imageObj);
                return imageObj.Id;
            }
#pragma warning restore IDE0063 // Use simple 'using' statement
        }

        public void SaveRecipe(Recipe recipe)
        {
            Crud.CreateRecipe(recipe);
        }

        public void UpdateRecipe(Recipe recipe)
        {
            Crud.UpdateRecipe(recipe);
        }

        public void DeletePicture(int id)
        {
            Crud.DeletePicture(id);
        }


        public Event AddParticipantToEvent(string name, string email, int id)
        {
            return Crud.AddParticipantToEvent(name, email, id);
        }

        public IEnumerable<KitchenType> GetAllKitchenTypes()
        {
            return Crud.ReadAllKitchenTypes();
        }
        public async Task<User> GetCurrentUser()
        {
            if (HttpContextAccessor.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }
            User? user = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext?.User!);
#pragma warning disable IDE0270 // Use coalesce expression
            if (user == null)
            {
                throw new ArgumentNullException("User unknown");
            }
#pragma warning restore IDE0270 // Use coalesce expression
            return user;
        }

        private int? GetCurrentUserId()
        {
            if (HttpContextAccessor.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }
            if (!int.TryParse(UserManager.GetUserId(HttpContextAccessor.HttpContext?.User!), out int userId))
            {
                return null;
            }
            return userId;
        }
    }
}