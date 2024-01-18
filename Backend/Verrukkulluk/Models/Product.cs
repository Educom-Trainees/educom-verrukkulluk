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
    }
}
