namespace Verrukkulluk.Models
{
    public interface IHomeModel
    {
        List<RecipeInfo>? Recipes { get; set; }
        List<Event>? Events { get; set; }
    }
}
