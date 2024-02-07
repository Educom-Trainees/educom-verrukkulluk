namespace Verrukkulluk
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Calories { get; set; }
        public double Amount { get; set; }
        public IngredientType IngredientType { get; set; }
        public int ImageObjId { get; set; }
        public string Description { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }

        public Product() { }
        public Product(string name, decimal price, double calories, double amount, IngredientType ingredientType, int imageObjId, string description)
        {
            Name = name;
            Price = price;
            Calories = calories;
            Amount = amount;
            IngredientType = ingredientType;
            ImageObjId = imageObjId;
            Description = description;
        }
    }
}
