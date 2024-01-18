using Microsoft.AspNetCore.Identity;

namespace Verrukkulluk.Models
{
    public class User : IdentityUser<int>
    {
        public List<Ingredient> ShoppingList { get; set; }
        public List<Recipe> FavouritesList { get; set; }
        public string CityOfResidence { get; set; }
    }
}
