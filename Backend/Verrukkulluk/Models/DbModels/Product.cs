using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
    public class Product
    {
        private readonly ILazyLoader _lazyLoader;
        private ICollection<ProductAllergy> productAllergies;
        private PackagingType packaging;

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Calories { get; set; }
        public double Amount { get; set; }
        public double SmallestAmount { get; set; }
        public int PackagingId { get; set; }
        [ForeignKey(nameof(PackagingId))]
        public PackagingType Packaging { get => _lazyLoader.Load(this, ref packaging); set => packaging = value; }
        public IngredientType IngredientType { get; set; }
        public int ImageObjId { get; set; }
        public string Description { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<ProductAllergy> ProductAllergies { get => _lazyLoader.Load(this, ref productAllergies); set => productAllergies = value; }

        public Product() { }
        public Product(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
        public Product(string name, decimal price, double calories, double amount, PackagingType packaging, IngredientType ingredientType, int imageObjId, string description, double smallestAmount = 1)
        {
            Name = name;
            Price = price;
            Calories = calories;
            Amount = amount;
            SmallestAmount = smallestAmount;
            Packaging = packaging;
            IngredientType = ingredientType;
            ImageObjId = imageObjId;
            Description = description;
        }
    }
}
