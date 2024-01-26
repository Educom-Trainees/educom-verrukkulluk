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
    }
}
