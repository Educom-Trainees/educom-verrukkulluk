using System.Collections.Generic;

namespace Verrukkulluk.Models
{
    public class RecipeInfo : Recipe
    {
        public string Price { get; set; }
        public int Calories { get; set; }
        public List<DishType> DishTypes { get; set; } = new List<DishType>();
        public List<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();
        public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

        public RecipeInfo() { }

        public RecipeInfo(Recipe recipe) : base(recipe.Title, recipe.KitchenType, recipe.Description, recipe.Instructions, recipe.AverageRating, recipe.Creator, recipe.ImageObjId, recipe.Ingredients.ToList(), recipe.NumberOfPeople)
        {
            Id = recipe.Id;
            Price = recipe.Ingredients.Select(i => i.Product.Price * (decimal)Math.Ceiling(i.Amount / i.Product.Amount)).Sum().ToString("F2");
            Calories = (int)recipe.Ingredients.Select(i => i.Product.Calories * i.Amount / i.Product.Amount).Sum()/NumberOfPeople;
            Allergies = Ingredients.Select(i => i.Product).SelectMany(p => p.ProductAllergies).Select(p => p.Allergy).Distinct().ToList();
            if (Allergies.Where(a => a.Name == "Vlees").Any())
            {
                DishTypes.Add(new DishType("Vlees"));
            }
            if (Allergies.Where(a => a.Name == "Vis").Any())
            {
                DishTypes.Add(new DishType("Vis"));
            }
            if (!(Allergies.Where(a => a.Name == "Vlees").Any() | Allergies.Where(a => a.Name == "Vis").Any() | Allergies.Where(a => a.Name == "Schaald").Any() | Allergies.Where(a => a.Name == "Weekdieren").Any()))
            {
                DishTypes.Add(new DishType("Vegetarisch"));
                if (!(Allergies.Where(a => a.Name == "Ei").Any() | Allergies.Where(a => a.Name == "Lactose").Any()))
                {
                    DishTypes.Add(new DishType("Vegan"));
                }
            }
            if (!Allergies.Where(a => a.Name == "Lactose").Any())
            {
                DishTypes.Add(new DishType("Lactosevrij"));
            }
            if (!Allergies.Where(a => a.Name == "Gluten").Any())
            {
                DishTypes.Add(new DishType("Glutenvrij"));
            }
        }
    }
}
