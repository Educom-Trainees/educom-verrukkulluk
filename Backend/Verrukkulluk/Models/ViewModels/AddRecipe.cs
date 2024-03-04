using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Verrukkulluk.Models;

namespace Verrukkulluk.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AddRecipe
    {
        public Recipe Recipe { get; set; }
        public bool DeleteImage { get; set; }
        [ValidateNever]
        public List<Product>? Products { get; set; }
        [Required(ErrorMessage = "Tenminste 1 ingredient moet worden ingevuld")]
        public Ingredient[] AddedIngredients { get; set; }
        [Display(Name = "Afbeelding")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage ="Afbeelding is verplicht.")]
        public IFormFile DishPhoto { get; set; } 
        public List<AllergyGroup> Allergygroups { get{
            return Recipe.Ingredients?.Select(i => i.Product).SelectMany(p => p.ProductAllergies).Select(p => p.Allergy)
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
        public List<SelectListItem> MyKitchenTypeOptions { get; } = new List<SelectListItem>() { 
            new SelectListItem() { Text = "- Type keuken -", Value = "0", Selected = true, Disabled = true } 
        };
        public AddRecipe() {
            Recipe = new Recipe();
        }

        public AddRecipe(Recipe recipe)  
        {
            Recipe = recipe;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}