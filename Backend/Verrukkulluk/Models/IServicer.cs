using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Models
{
        public interface IServicer
        {
                List<Product> GetAllProducts();
                Product? GetProductById(int productId);
                void DeleteUserRecipe(int id);
                List<RecipeInfo> GetAllRecipes();
                List<RecipeInfo> GetUserRecipes();
                List<RecipeInfo> GetRecipesByUserId(int userId);
                List<RecipeInfo> GetUserFavorites();
                RecipeInfo GetRecipeById(int Id);
                Task<SignInResult> Login(InputModel input);
                ImageObj GetImage(int Id);
                bool RateRecipe(int recipeId, int ratingValue, string comment);
                int? GetUserRating(int recipeId);
                List<RecipeRating> GetRatingsByUserId(int userId);
                string? GetUserComment(int recipeId);
                Event GetEventById(int Id);
                List<Event> GetAllEvents();
                void UpdateAverageRating(int recipeId);
                Task<int> SavePictureAsync(IFormFile picture);
                void SaveRecipe(Recipe recipe);
                void UpdateRecipe(Recipe recipe);
                void DeletePicture(int imageObjId);
                void SaveProfilePicture(ImageObj image, User user);
                Event AddParticipantToEvent(string name, string email, int id);
        }
}
