using System.ComponentModel.DataAnnotations;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AddRecipe : Recipe {

        public List<Product>? Products { get; set; }

        public Ingredient[] AddedIngredients { get; set; }
        
        [Display(Name = "Afbeelding")]
        public byte[] DishPhoto { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}