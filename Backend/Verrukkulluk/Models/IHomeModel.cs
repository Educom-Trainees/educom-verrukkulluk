namespace Verrukkulluk.Models
{
    public interface IHomeModel
    {
        List<Recipe>? Recipes { get; set; }
        void GetAllRecipes();
    }
}
