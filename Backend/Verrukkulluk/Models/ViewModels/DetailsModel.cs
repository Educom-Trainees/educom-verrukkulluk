using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class DetailsModel:VerModel, IDetailsModel
    {
        public Recipe Recipe { get; set; }
        public double Calories { get; set; }
        public decimal Price { get; set; }
        public String base64RecipePicture { get; set; }


        public DetailsModel() { }
        public DetailsModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}
