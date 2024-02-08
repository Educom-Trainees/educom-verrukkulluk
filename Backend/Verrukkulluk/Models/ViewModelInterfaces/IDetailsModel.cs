namespace Verrukkulluk.Models
{
    public interface IDetailsModel
    {
        RecipeInfo Recipe { get; set; }
        double Calories { get; set; }
        decimal Price { get; set; }
    }
}
