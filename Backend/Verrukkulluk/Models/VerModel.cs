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
        public List<Recipe>? Recipes { get; set; }
        

        public VerModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            Crud = crud;
            UserManager = userManager;
            HttpContextAccessor = httpContextAccessor;
        }

        public void GetUserRecipes()
        {
            //var tempUserId = UserManager.GetUserId(HttpContextAccessor.HttpContext.User);
            //int userId = int.Parse(tempUserId);
            int userId = 1;
            Recipes = Crud.ReadAllRecipesByUserId(userId);
        }
    }
}
