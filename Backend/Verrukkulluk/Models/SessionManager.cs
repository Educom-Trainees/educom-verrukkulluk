using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Data;

namespace Verrukkulluk.Models
{
    public class SessionManager: ISessionManager
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        public SessionManager()
        {

        }

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public List<CartItem> GetShoppingList()
        {
            var shoppingList = HttpContextAccessor.HttpContext.Session.Get<List<CartItem>>("ShoppingList") ?? new List<CartItem>();
            return shoppingList;
        }
        public void SaveShoppingList(List<CartItem> shoppingList)
        {
            HttpContextAccessor.HttpContext.Session.Set("ShoppingList", shoppingList);
        }

        public string AddRecipeToShoppingList(Recipe Recipe)
        {
            if (Recipe == null)
            {
                return "fail";
            }

            var existingShoppingList = HttpContextAccessor.HttpContext.Session.Get<List<CartItem>>("ShoppingList");
            var shoppingList = existingShoppingList ?? new List<CartItem>();

            foreach (var ingredient in Recipe.Ingredients)
            {
                double quantityNeeded = ingredient.Amount / ingredient.Product.Amount;
                var newItem = new CartItem
                {
                    ImageObjId = ingredient.Product.ImageObjId, //Ik weet niet hoe dit meegegeven wordt
                    Name = ingredient.Product.Name,
                    Description = ingredient.Product.Description,
                    Quantity = Math.Round(quantityNeeded, 2),
                    Price = ingredient.Product.Price * (decimal)quantityNeeded
                };
                shoppingList.Add(newItem);
            }
            HttpContextAccessor.HttpContext.Session.Set("ShoppingList", shoppingList);
            return "success";
        }
    }
}
