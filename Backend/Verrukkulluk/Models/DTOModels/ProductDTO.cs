namespace Verrukkulluk.Models.DTOModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Calories { get; set; }
        public double Amount { get; set; }
        public double SmallestAmount { get; set; }
        public string PackagingTypeName { get; set; }
        public IngredientType IngredientType { get; set; }
        public string Description { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public List<string> ProductAllergies { get; set; }

    }
}
