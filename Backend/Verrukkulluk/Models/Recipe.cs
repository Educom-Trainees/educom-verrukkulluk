using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
    public class Recipe
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }         
        public List<RecipeDishType> RecipeDishTypes { get; set; } 
        public KitchenType KitchenType { get; set; }
        [MaxLength(1000)]
        public string Instructions { get; set; }
        virtual public ICollection<Comment>? Comments { get; set; } = null;
        public int Rating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; }
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
}