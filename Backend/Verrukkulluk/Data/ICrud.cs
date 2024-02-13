
using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;

namespace Verrukkulluk.Data
{
    public interface ICrud
    {
        List<Product> ReadAllProducts();
        string DeleteUserRecipe(int userId, int recipeId);
        List<RecipeInfo>? ReadAllRecipes();
        List<RecipeInfo>? ReadAllRecipesByUserId(int userId);
        Product? ReadProductById(int id);
        public RecipeInfo ReadRecipeById(int Id);
        public ImageObj ReadImageById(int Id);
        public Event ReadEventById(int Id);
        List<Event> ReadAllEvents();
        bool AddRecipeRating(int recipeId, int userId, int ratingValue);
        int? ReadUserRating(int recipeId, int userId);
    }
}