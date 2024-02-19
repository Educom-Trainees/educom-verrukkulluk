using Microsoft.AspNetCore.Identity;

namespace Verrukkulluk.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public List<Ingredient> ShoppingList { get; set; }
        public List<Recipe> FavouritesList { get; set; }
        public string CityOfResidence { get; set; }
        public int ImageObjId { get; set; }

        public User() { }
        public User(string email, string firstName, string cityOfResidence, int imageObjId)
        {
            Email = email;
            FirstName = firstName;
            UserName = firstName;
            CityOfResidence = cityOfResidence;
            ImageObjId = imageObjId;
        }
        
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
