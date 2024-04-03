using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class SessionManager: ISessionManager
    {
        private readonly ISession? session;
        
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext?.Session;
        }
        public List<CartItem> GetShoppingList()
        {
            var shoppingList = session?.Get<List<CartItem>>("ShoppingList") ?? new List<CartItem>();
            return shoppingList;
        }
        public void SaveShoppingList(List<CartItem> shoppingList)
        {
            session?.Set("ShoppingList", shoppingList);
        }

        public string AddRecipeToShoppingList(Recipe Recipe)
        {
            if (Recipe == null)
            {
                return "fail";
            }

            var shoppingList = (session?.Get<List<CartItem>>("ShoppingList")) ?? new List<CartItem>();

            foreach (var ingredient in Recipe.Ingredients)
            {
                double quantityNeeded = ingredient.Amount / ingredient.Product.Amount;
                var newItem = new CartItem
                {
                    ImageObjId = ingredient.Product.ImageObjId,
                    Name = ingredient.Product.Name,
                    Description = ingredient.Product.Description,
                    Quantity = quantityNeeded,
                    Price = ingredient.Product.Price
                };
                shoppingList.Add(newItem);
            }
            session?.Set("ShoppingList", shoppingList);
            return "success";
        }

        /// <summary>
        /// Add a rating for a recipe for the current anonymous user
        /// </summary>
        /// <param name="recipeId">The id of the recipe</param>
        /// <param name="ratingValue">The rating</param>
        public void AddRecipeRating(int recipeId, int ratingValue)
        {
            var sessionRatings = session?.Get<Dictionary<int, int>>("SessionRatings") ?? new Dictionary<int, int>();
            sessionRatings[recipeId] = ratingValue;
            session?.Set("SessionRatings", sessionRatings);
        }

        /// <summary>
        /// Return the current rating for this recipe for this anonymous user or <code>null</code> when not rated
        /// </summary>
        /// <param name="recipeId">The id of the recipe</param>
        /// <returns>The recipe rating or <code>null</code> if not found</returns>
        public int? GetRecipeRating(int recipeId)
        {
            var sessionRatings = session?.Get<Dictionary<int, int>>("SessionRatings");
            if (sessionRatings != null && sessionRatings.TryGetValue(recipeId, out int value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
