﻿using System.Collections.Generic;

namespace Verrukkulluk.Models
{
    public class RecipeInfo : Recipe
    {
        public decimal Price { get; set; }
        public double Calories { get; set; }

        public RecipeInfo() { }
        public RecipeInfo(string title, List<RecipeDishType> recipeDishTypes, KitchenType kitchenType, string description, string instructions, int rating, User creator, int imageObjId, List<Ingredient> ingredients) :
                     base(title, recipeDishTypes, kitchenType, description, instructions, rating, creator, imageObjId, ingredients)
        {
            Price = ingredients.Select(i => i.Product.Price * (decimal)Math.Ceiling(i.Amount)).Sum();
            Calories = ingredients.Select(i => i.Product.Calories * i.Amount).Sum();
        }

        public RecipeInfo(Recipe recipe) : base(recipe.Title, recipe.RecipeDishTypes, recipe.KitchenType, recipe.Description, recipe.Instructions, recipe.Rating, recipe.Creator, recipe.ImageObjId, recipe.Ingredients.ToList())
        {
            Id = recipe.Id;
            Price = recipe.Ingredients.Select(i => i.Product.Price * (decimal)Math.Ceiling(i.Amount)).Sum();
            Calories = recipe.Ingredients.Select(i => i.Product.Calories * i.Amount).Sum();
        }
    }
}
