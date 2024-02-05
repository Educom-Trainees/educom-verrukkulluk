using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class FavoritesModel : VerModel, IFavoritesModel
    {
        public List<Recipe>? Recipes { get; set; }

        public void GetUserFavorites()
        {
            if (HttpContextAccessor?.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }
            var tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            if (int.TryParse(tempUserId, out int userId))
            {
                //Hoe de favorieten uit de crud gehaald moeten worden. !Nog aanpassen! Nu zijn het de recepten die iemand zelf gemaakt heeft
                Recipes = Crud.ReadAllRecipesByUserId(userId);
            }
        }

        public FavoritesModel() { }

        public FavoritesModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}