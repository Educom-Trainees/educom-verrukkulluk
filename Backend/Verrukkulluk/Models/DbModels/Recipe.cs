using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DbModels;

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
        public List<RecipeDishType> RecipeDishTypes { get; set; } = new List<RecipeDishType>();
        public KitchenType KitchenType { get; set; }
        public List<Allergy> Allergies { get; set; } = new List<Allergy>();
        [MaxLength(1000)]
        public string Instructions { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public int Rating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public int ImageObjId { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public ICollection<ProductAllergy> ProductAllergies { get; set; } = new List<ProductAllergy>();

        public Recipe() { }
        public Recipe(string title, List<RecipeDishType> recipeDishTypes, KitchenType kitchenType, string description, string instructions, int rating, User creator, int imageObjId, List<Ingredient> ingredients)
        {
            Title = title;
            RecipeDishTypes = recipeDishTypes;
            KitchenType = kitchenType;
            Description = description;
            Instructions = instructions;
            Rating = rating;
            CreatorId = creator.Id;
            Creator = creator;
            ImageObjId = imageObjId;
            Ingredients = ingredients;

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            CreationDate = currentDate;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}