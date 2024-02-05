namespace Verrukkulluk.Models
{
    public interface IDetailsModel
    {
        void GetCalories(int Id);
        void GetPrice(int Id);
        void GetRecipeById(int Id);
    }
}
