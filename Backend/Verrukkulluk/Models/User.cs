using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Verrukkulluk.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class User : IdentityUser<int>
    {
        private readonly ILazyLoader _lazyLoader; // implement lazy loading for the favourites
        private List<Recipe>? favouritesList = null;

        public string FirstName { get; set; }
        [ValidateNever]
        public List<Recipe>? FavouritesList { 
                  get => _lazyLoader.Load(this, ref favouritesList); 
                  set => favouritesList = value; }
        public string CityOfResidence { get; set; }
        public int ImageObjId { get; set; }

        public User() { }

        public User(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
        public User(string email, string firstName, string cityOfResidence, int imageObjId, string phoneNumber)
        {
            Email = email;
            FirstName = firstName;
            UserName = email;
            CityOfResidence = cityOfResidence;
            ImageObjId = imageObjId;
            PhoneNumber = phoneNumber;
        }
        
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
