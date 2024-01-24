
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IVerModel
    {
        List<Recipe>? Recipes { get; set; }

        void GetUserRecipes();
    }
}