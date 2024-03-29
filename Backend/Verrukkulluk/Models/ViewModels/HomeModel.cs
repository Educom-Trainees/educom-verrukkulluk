﻿using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class HomeModel:VerModel,IHomeModel
    {
        public List<RecipeInfo>? Recipes { get; set; }
        public List<Event>? Events { get; set; }

        public HomeModel() { }

        public HomeModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        {}
    }
}
