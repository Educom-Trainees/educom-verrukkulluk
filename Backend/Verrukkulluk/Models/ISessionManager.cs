namespace Verrukkulluk.Models
{
    public interface ISessionManager
    {
        List<CartItem> GetShoppingList();
        void SaveShoppingList(List<CartItem> shoppingList);
        string AddRecipeToShoppingList(Recipe Recipe);
    }
}
