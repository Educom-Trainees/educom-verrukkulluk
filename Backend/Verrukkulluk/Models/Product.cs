namespace Verrukkulluk
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public double Amount { get; set; }
        public IngredientType IngredientType { get; set; }
        public string PhotoLocation { get; set; }

        public Product() { }
        public Product(string name, decimal price, int calories, double amount, IngredientType ingredientType, string photoLocation)
        {
            Name = name;
            Price = price;
            Calories = calories;
            Amount = amount;
            IngredientType = ingredientType;
            PhotoLocation = photoLocation;
        }
    }
}
