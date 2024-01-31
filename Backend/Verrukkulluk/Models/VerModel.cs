using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class VerModel : IVerModel
    {
        private readonly ICrud Crud;
        private readonly UserManager<User> UserManager;
        private readonly IHttpContextAccessor HttpContextAccessor;
        public readonly SignInManager<User> SignInManager;
        public string Error {  get; set; }
        public List<Recipe>? Recipes { get; set; }
        public InputModel Input { get; set; } = new InputModel();
        public VerModel()
        {
             
        }

        public VerModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            Crud = crud;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
            SignInManager = signInManager;
        }

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

        public async Task<SignInResult> Login(InputModel input)
        {
            return await SignInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
        }

    }
}
