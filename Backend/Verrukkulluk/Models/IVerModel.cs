
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IVerModel
    {
        List<Recipe>? Recipes { get; set; }

        InputModel Input { get; set; }

        void GetUserRecipes();

        Task<SignInResult> Login(InputModel input);
    }
}