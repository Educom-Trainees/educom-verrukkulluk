﻿using Microsoft.EntityFrameworkCore;

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
