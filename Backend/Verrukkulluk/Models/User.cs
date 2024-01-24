﻿using Microsoft.AspNetCore.Identity;

namespace Verrukkulluk.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public List<Ingredient> ShoppingList { get; set; }
        public List<Recipe> FavouritesList { get; set; }
        public string CityOfResidence { get; set; }

        public User() { }
        public User(string email, string firstName, string cityOfResidence)
        {
            Email = email;
            FirstName = firstName;
            CityOfResidence = cityOfResidence;
        }
        public User(int id, string email, string firstName, string cityOfResidence)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            CityOfResidence = cityOfResidence;
        }
    }
}
