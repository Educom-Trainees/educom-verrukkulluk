using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Verrukkulluk
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0.01, 50_000.0, ErrorMessage = "Vul een aantal tussen 1 en 50.000 in")]
        public double Amount { get; set; }
        public bool Acquired { get; set; } = false;

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        public int RecipeId { get; set; }
        [ValidateNever]
        public Recipe Recipe { get; set; }

        public Ingredient() { }
        public Ingredient(string name, double amount, Product product)
        {
            Name = name;
            Amount = amount;
            ProductId = product.Id;
            Product = product;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}