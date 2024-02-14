﻿using Microsoft.EntityFrameworkCore;
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
                .Include(r => r.Creator);
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

        public Event ReadEventById(int Id)
        {
            return Context.Events.Where(e => e.Id == Id).First();
        }

        public List<Event> ReadAllEvents()
        {
            return Context.Events.ToList();
        }

        public bool AddOrUpdateRecipeRating(int recipeId, int? userId, int ratingValue)
        {
            try
            {
                var existingRating = Context.RecipeRatings.FirstOrDefault(r => r.RecipeId == recipeId && r.UserId == userId);

                if (existingRating != null)
                {
                    existingRating.RatingValue = ratingValue;
                }
                else
                {
                    Context.RecipeRatings.Add(new RecipeRating
                    {
                        RecipeId = recipeId,
                        UserId = userId,
                        RatingValue = ratingValue
                    });
                }
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        public int? ReadUserRating(int recipeId, int userId)
        {
            var rating = Context.RecipeRatings
                .FirstOrDefault(r => r.RecipeId == recipeId && r.UserId == userId);

            return rating?.RatingValue;
        }

        public double? GetAverageRating(int recipeId)
        {
            var ratings = Context.RecipeRatings.Where(r => r.RecipeId == recipeId).Select(r => r.RatingValue);
            if (ratings.Any())
            {
                return Math.Round(ratings.Average(), 2);
            }
            else
            {
                return 0;
            }
        }

        public void UpdateAverageRating(int recipeId)
        {
            var averageRating = GetAverageRating(recipeId);
            if (averageRating != null)
            {
                var recipe = Context.Recipes.Find(recipeId);
                recipe.AverageRating = (double)averageRating;
                Context.SaveChanges();
            }
        }
    }
}
