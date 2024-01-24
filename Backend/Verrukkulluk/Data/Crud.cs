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

        public List<Recipe>? ReadAllRecipesByUserId(int userId)
        {
            try
            {
                var recipes = Context.Recipes
                    .Include(d => d.DishType)
                    .Include(k => k.KitchenType)
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
