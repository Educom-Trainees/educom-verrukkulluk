
namespace Verrukkulluk.Data
{
    public interface ICrud
    {
        List<Recipe>? ReadAllRecipesByUserId(int userId);
    }
}