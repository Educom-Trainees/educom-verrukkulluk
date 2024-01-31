
using Microsoft.AspNetCore.Mvc;

namespace Verrukkulluk.Data
{
    public interface ICrud
    {
        List<Product> ReadAllProducts();
        string DeleteUserRecipe(int userId, int recipeId);
        List<Recipe>? ReadAllRecipesByUserId(int userId);
        Product? ReadProductById(int id);
        public double ReadCaloriesByRecipeId(int recipeId);
        public double ReadPriceByRecipeId(int recipeId);
    }
}