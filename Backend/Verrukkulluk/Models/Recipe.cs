using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Recipe
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }  
        [MaxLength(500)]       
        public string Description { get; set; }
        public List<RecipeDishType> RecipeDishTypes { get; set; } 
        public KitchenType KitchenType { get; set; }
        public List<Allergy> Allergies { get; set; }
        [MaxLength(1000)]
        public string Instructions { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; } = null;
        public int Rating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [Display(Name = "Afbeelding")]
        public byte[] DishPhoto { get; set; }
        public string PhotoLocation { get; set; }
        public ICollection<Ingredient> Ingredient { get; set; }

        public Recipe() { }
        public Recipe(string title, List<RecipeDishType> recipeDishTypes, KitchenType kitchenType, string instructions, int rating, User creator, string photoLocation, List<Ingredient> ingredient)
        {
            Title = title;
            RecipeDishTypes = recipeDishTypes;
            KitchenType = kitchenType;
            Instructions = instructions;
            Rating = rating;
            CreatorId = creator.Id;
            Creator = creator;
            PhotoLocation = photoLocation;
            Ingredient = ingredient;

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            CreationDate = currentDate;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}