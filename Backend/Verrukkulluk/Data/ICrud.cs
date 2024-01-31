
namespace Verrukkulluk.Data
{
    public interface ICrud
    {
        string DeleteUserRecipe(int userId, int recipeId);
        List<Recipe>? ReadAllRecipesByUserId(int userId);
    }
}