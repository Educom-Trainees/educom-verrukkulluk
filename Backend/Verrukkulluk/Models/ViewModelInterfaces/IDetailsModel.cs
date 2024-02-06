namespace Verrukkulluk.Models
{
    public interface IDetailsModel
    {
        Recipe Recipe { get; set; }
        double Calories { get; set; }
        decimal Price { get; set; }
    }
}
