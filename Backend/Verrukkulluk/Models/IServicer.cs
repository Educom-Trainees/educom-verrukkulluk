﻿using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;
using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Models
{
        public interface IServicer
        {
                List<Product> GetAllProducts();
                Product? GetProductById(int productId);
                Task<bool> DeleteUserRecipeAsync(int id);
                List<RecipeInfo> GetAllRecipes();
                List<RecipeInfo> GetUserRecipes();
                List<RecipeInfo> GetRecipesByUserId(int userId);
                Task<List<RecipeInfo>> GetUserFavorites();
                Recipe? GetRecipeById(int id);
                RecipeInfo? GetRecipeInfoById(int id);
                Task<SignInResult> Login(InputModel input);
                ImageObj? GetImage(int Id);
                bool RateRecipe(int recipeId, int ratingValue, string comment);
                int? GetUserRating(int recipeId);
                List<RecipeRating> GetRatingsByUserId(int userId);
                string? GetUserComment(int recipeId);
                Event GetEventById(int id);
                List<Event> GetAllEvents();
                List<Event> GetUserEvents(string userEmail);
                Task<List<Event>> GetCurrentUserEvents();
                void UpdateAverageRating(int recipeId);
                Task<int> SavePictureAsync(IFormFile picture);
                void SaveRecipe(Recipe recipe);
                void UpdateRecipe(Recipe recipe);
                void DeletePicture(int imageObjId);
                bool AddParticipantToEvent(string name, string email, int id);
                bool RemoveParticipantFromEvent(string userEmail, int eventId);
                IEnumerable<KitchenType> GetAllActiveKitchenTypes();
                Task<User> GetCurrentUser();
    }
}
