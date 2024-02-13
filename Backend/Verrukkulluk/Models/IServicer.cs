﻿using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public interface IServicer
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int productId);
        void DeleteUserRecipe(int id);
        List<RecipeInfo> GetAllRecipes();
        List<RecipeInfo> GetUserRecipes();
        List<RecipeInfo> GetUserFavorites();
        RecipeInfo GetRecipeById(int Id);
        Task<SignInResult> Login(InputModel input);
        ImageObj GetImage(int Id);
        bool RateRecipe(int recipeId, int ratingValue);
        int? GetUserRating(int recipeId);
        Event GetEventById(int Id);
        List<Event> GetAllEvents();
    }
}
