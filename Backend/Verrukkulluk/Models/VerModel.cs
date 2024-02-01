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
        public Utils Utils { get; set; } = new Utils();
        public Recipe Recipe { get; set; }
        public double Calories { get; set; }
        public decimal Price { get; set; }
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

        public List<Product> GetAllProducts() {
            return Crud.ReadAllProducts();
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

        public Product? GetProductById(int productId)
        {
            return Crud.ReadProductById(productId);
        }

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
        }
    }
}
