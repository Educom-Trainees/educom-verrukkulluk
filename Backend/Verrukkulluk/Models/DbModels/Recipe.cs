using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
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
        public KitchenType KitchenType { get; set; }
        public List<Allergy> Allergies { get; set; } = new List<Allergy>();
        [MaxLength(1000)]
        public string[] Instructions { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public double AverageRating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int ImageObjId { get; set; }
        public int NumberOfPeople { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<ProductAllergy> ProductAllergies { get; set; } = new List<ProductAllergy>();
        public ICollection<RecipeRating> Ratings { get; set; } = new List<RecipeRating>();

        public Recipe() { }
        public Recipe(string title, KitchenType kitchenType, string description, string[] instructions, double rating, User creator, int imageObjId, List<Ingredient> ingredients, int numberOfPeople)
        {
            Title = title;
            KitchenType = kitchenType;
            Description = description;
            Instructions = instructions;
            AverageRating = rating;
            CreatorId = creator.Id;
            Creator = creator;
            ImageObjId = imageObjId;
            Ingredients = ingredients;

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            CreationDate = currentDate;
            NumberOfPeople = numberOfPeople;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}