using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Numerics;
using Verrukkulluk.Models;

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

        public List<RecipeInfo>? ReadAllRecipes()
        {
            try 
            {
                var recipes = Context.Recipes
                    .Include(r => r.RecipeDishTypes)
                        .ThenInclude(r => r.DishType)
                    .Include(r => r.KitchenType)
                    .Include(r => r.Creator)
                    .Include(r => r.Ingredients)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.ProductAllergies)
                                .ThenInclude(p => p.Allergy)
                    .OrderBy(recipe => recipe.CreationDate)
                    .Select(r => new RecipeInfo(r))
                    .ToList();
                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            return null;
        }

        public List<RecipeInfo>? ReadAllRecipesByUserId(int userId)
        {
            try
            {
                var recipes = Context.Recipes
                    .Include(r => r.RecipeDishTypes)
                        .ThenInclude(r => r.DishType)
                    .Include(r => r.KitchenType)
                    .Include(r => r.Creator)
                    .Include(r => r.Ingredients)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.ProductAllergies)
                                .ThenInclude(p => p.Allergy)
                    .Where(recipe => recipe.CreatorId == userId)
                    .Select(r => new RecipeInfo(r))
                    .ToList();
                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            return null;
        }

        public RecipeInfo ReadRecipeById(int Id)
        {
            var Recipes = Context.Recipes
                .Where(i => i.Id == Id)
                .Include(r => r.KitchenType)
                .Include(r => r.Creator)
                .Include(r => r.RecipeDishTypes)
                    .ThenInclude(r => r.DishType);
            var Recipe = Recipes
                .Include(r => r.Comments)
                .Include(r => r.Ingredients)
                    .ThenInclude(r => r.Product)
                        .ThenInclude(p => p.ProductAllergies)
                                .ThenInclude(p => p.Allergy)
                .First();
            return new RecipeInfo(Recipe);
        }

        public ImageObj ReadImageById(int Id)
        {
            return Context.ImageObjs.Where(i => i.Id == Id).First();
        }
    }
}
