using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class UserRecipesModel : VerModel, IUserRecipesModel
    {
        public List<RecipeInfo>? Recipes { get; set; }

        public UserRecipesModel() { }

        public UserRecipesModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}