using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class UserRecipesModel : VerModel, IUserRecipesModel
    {
        public List<RecipeInfo>? Recipes { get; set; }
        public void GetUserRecipes()
        {
            if (HttpContextAccessor?.HttpContext?.User == null)
            {
                throw new ArgumentNullException("HttpContextAccessor.HttpContext.User");
            }
            var tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            if (int.TryParse(tempUserId, out int userId))
            {
                Recipes = Crud.ReadAllRecipesByUserId(userId);
            }
        }
        public void DeleteUserRecipe(int recipeId)
        {
            string tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            int userId = int.Parse(tempUserId);

            //UserId is supplied so that only the recipe's creator can delete their recipe
            string result = Crud.DeleteUserRecipe(userId, recipeId);

            if (result != "Recept verwijderd.")
            {
                Error = result;
            }
        }

        public UserRecipesModel() { }

        public UserRecipesModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}