using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Verrukkulluk.Models;

namespace Verrukkulluk.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AddRecipe : Recipe 
    {      
        [Range(1, 20, ErrorMessage = "Dit gerecht is alleen voor 1 tot 20 personen.")]
        public int NumberOfPeople { get; set; }
        public List<Product>? Products { get; set; }
        public Ingredient[] AddedIngredients { get; set; }
        [Display(Name = "Afbeelding")]
        public IFormFile DishPhoto { get; set; } 
        public List<AllergyGroup> Allergygroups { get{
            return Ingredients?.Select(i => i.Product).SelectMany(p => p.ProductAllergies).Select(p => p.Allergy)
                               .GroupBy(a => a.Id, // Where do we group by
                                        a => a, // What elements are in the group
                                        (id, allergies) => // What do we return
                                        new AllergyGroup { 
                                            Id = id, 
                                            Count = allergies.Count(), 
                                            Allergy = allergies.First()
                                            }
                                        ).ToList() ?? new List<AllergyGroup>();
        }}   
        public AddRecipe() {
            NumberOfPeople = 4;
        }

        public AddRecipe(string title, string description, string[] addedInstructionSteps, Ingredient[] addedIngredients, int numberOfPeople, KitchenType kitchenType, IFormFile dishPhoto, User creator)  
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