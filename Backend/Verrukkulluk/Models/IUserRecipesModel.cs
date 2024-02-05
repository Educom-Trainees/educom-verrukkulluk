using Verrukkulluk;

namespace Verrukkulluk.Models
{
    public interface IUserRecipesModel
    {
        public List<Recipe>? Recipes { get; set; }
        void GetUserRecipes();
        void DeleteUserRecipe(int id);
    }
}
