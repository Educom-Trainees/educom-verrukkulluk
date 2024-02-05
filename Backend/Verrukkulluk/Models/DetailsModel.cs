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


        public void GetCalories(int Id)
        {
            Calories = Crud.ReadCaloriesByRecipeId(Id);
        }

        public void GetPrice(int Id)
        {
            Price = (decimal)Crud.ReadPriceByRecipeId(Id);
        }

        public void GetRecipeById(int Id)
        {
            Recipe = Crud.ReadRecipeById(Id);
            base64RecipePicture = Convert.ToBase64String(Recipe.DishPhoto);
        }


        public DetailsModel() { }
        public DetailsModel(ICrud crud, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager) : base(crud, userManager, httpContextAccessor, signInManager)
        { }
    }
}
