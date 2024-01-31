
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IVerModel
    {
        string Error { get; set; }
        List<Recipe>? Recipes { get; set; }
        InputModel Input { get; set; }

        List<Product> GetAllProducts();
        Product? GetProductById(int productId);
        void DeleteUserRecipe(int id);
        void GetUserRecipes();
        Task<SignInResult> Login(InputModel input);
    }
}