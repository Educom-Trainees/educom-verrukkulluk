using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Verrukkulluk.Models;

namespace Verrukkulluk.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AddRecipe : Recipe 
    {      
        public List<Product>? Products { get; set; }
        public Ingredient[] AddedIngredients { get; set; }
        [Display(Name = "Afbeelding")]
        public byte[] DishPhoto { get; set; } 
        public List<Allergy> Allergies { get{
            return Ingredients?.Select(i => i.Product).SelectMany(p => p.ProductAllergies).Select(p => p.Allergy).Distinct().ToList() ?? new List<Allergy>();
        }}   
        public AddRecipe() {}

        public AddRecipe(string title, string description, string[] addedInstructionSteps, Ingredient[] addedIngredients, int numberOfPeople, KitchenType kitchenType, byte[] dishPhoto, User creator)  
        {
            Title = title;
            Description = description;
            Instructions = addedInstructionSteps;
            Ingredients = addedIngredients;
            NumberOfPeople = numberOfPeople;
            KitchenType = kitchenType;
            DishPhoto = dishPhoto;
            Creator = creator;
            CreatorId = creator.Id;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}