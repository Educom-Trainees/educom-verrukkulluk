using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IServicer
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int productId);
        void DeleteUserRecipe(int id);
        List<Recipe> GetAllRecipes();
        List<Recipe> GetUserRecipes();
        List<Recipe> GetUserFavorites();
        double GetCalories(int Id);
        decimal GetPrice(int Id);
        Recipe GetRecipeById(int Id);
        Task<SignInResult> Login(InputModel input);
    }
}
