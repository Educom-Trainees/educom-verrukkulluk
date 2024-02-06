namespace Verrukkulluk.Models
{
    public interface IFavoritesModel
    {
        public List<Recipe>? Recipes { get; set; }
        void GetUserFavorites();
    }
}
