using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class DetailsModel:VerModel, IDetailsModel
    {
        public RecipeInfo Recipe { get; set; }


        public DetailsModel() { }
        public DetailsModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}
