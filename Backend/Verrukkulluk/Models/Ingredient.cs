using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public IngredientType IngredientType { get; set; }
        public bool Acquired { get; set; } = false;

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public Ingredient() { }
        public Ingredient(string name, double amount, IngredientType ingredientType, Product product)
        {
            Name = name;
            Amount = amount;
            IngredientType = ingredientType;
            ProductId = product.Id;
            Product = product;
        }
    }
}