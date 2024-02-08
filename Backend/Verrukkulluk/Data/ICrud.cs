
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
        public double ReadCaloriesByRecipeId(int recipeId);
        public double ReadPriceByRecipeId(int recipeId);
        public RecipeInfo ReadRecipeById(int Id);
        public ImageObj ReadImageById(int Id);
    }
}