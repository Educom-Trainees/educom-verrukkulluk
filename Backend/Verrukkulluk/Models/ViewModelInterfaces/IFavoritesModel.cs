namespace Verrukkulluk.Models
{
    public interface IFavoritesModel
    {
        public List<RecipeInfo>? Recipes { get; set; }
        void GetUserFavorites();
    }
}
