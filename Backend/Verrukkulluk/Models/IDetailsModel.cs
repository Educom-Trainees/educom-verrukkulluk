namespace Verrukkulluk.Models
{
    public interface IDetailsModel
    {
        Recipe Recipe { get; set; }
        void GetCalories(int Id);
        void GetPrice(int Id);
        void GetRecipeById(int Id);
    }
}
