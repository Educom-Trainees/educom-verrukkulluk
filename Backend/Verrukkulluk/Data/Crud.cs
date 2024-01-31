using Microsoft.EntityFrameworkCore;

namespace Verrukkulluk.Data
{
    public class Crud : ICrud
    {
        VerrukkullukContext Context;

        public Crud(VerrukkullukContext context)
        {
            Context = context;
        }

        public List<Product> ReadAllProducts()
        {
            return Context.Products.ToList();
        }
        public Product? ReadProductById(int id)
        {
            return Context.Products.Find(id);
        }
        public string DeleteUserRecipe(int userId, int recipeId)
        {
            try
            {
                var selectedRecipe = Context.Recipes
                    .Where(recipe => recipe.CreatorId == userId)
                    .FirstOrDefault(recipe => recipe.Id == recipeId);

                if (selectedRecipe != null)
                {
                    Context.Recipes.Remove(selectedRecipe);
                    Context.SaveChanges();
                    return "Recept verwijderd.";
                } else
                {
                    return "Recept kon niet worden gevonden.";
                }
            } catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return "Er ging iets mis. Probeer het later opnieuw";
            }
        }

        public List<Recipe>? ReadAllRecipesByUserId(int userId)
        {
            try
            {
                var recipes = Context.Recipes
                    .Include(r => r.RecipeDishTypes)
                        .ThenInclude(r => r.DishType)
                    .Include(r => r.KitchenType)
                    .Where(recipe => recipe.CreatorId == userId)
                    .ToList();
                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            return null;
        }
    }
}
