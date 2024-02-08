using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IServicer
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int productId);
        void DeleteUserRecipe(int id);
        List<RecipeInfo> GetAllRecipes();
        List<RecipeInfo> GetUserRecipes();
        List<RecipeInfo> GetUserFavorites();
        double GetCalories(int Id);
        decimal GetPrice(int Id);
        RecipeInfo GetRecipeById(int Id);
        Task<SignInResult> Login(InputModel input);
        ImageObj GetImage(int Id);
    }
}
