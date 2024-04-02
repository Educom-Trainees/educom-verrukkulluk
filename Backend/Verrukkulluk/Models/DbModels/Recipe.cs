using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Verrukkulluk.Models;

namespace Verrukkulluk
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Recipe
    {
        private ILazyLoader _lazyLoader; // implement lazy loading for the favourites
        private KitchenType kitchenType;
        private ICollection<RecipeRating>? ratings = new List<RecipeRating>();
        private ICollection<Ingredient> ingredients = new List<Ingredient>();

        public int Id { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage ="Titel is verplicht.")]
        public string Title { get; set; }  
        [MaxLength(500)]       
        [Required(ErrorMessage ="Beschrijving is verplicht.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Type keuken is verplicht.")]
        public int KitchenTypeId { get; set; }
        [ForeignKey(nameof(KitchenTypeId))]
        [ValidateNever]
        public KitchenType KitchenType { get => _lazyLoader.Load(this, ref kitchenType); set => kitchenType = value; }
        [MaxLength(1000)]
        [Required(ErrorMessage ="Beschrijf tenminste stap 1.")]
        public string[] Instructions { get; set; }
        virtual public ICollection<RecipeRating>? Ratings { get => _lazyLoader.Load(this, ref ratings); set => ratings = value; }
        public double AverageRating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int CreatorId { get; set; }
        [ValidateNever]
        public User Creator { get; set; }
        public int ImageObjId { get; set; }
        [Range(1, 20, ErrorMessage = "Kies een aantal van 1 tot 20 personen.")]
        public int NumberOfPeople { get; set; } = 4;
        public ICollection<Ingredient> Ingredients { get => _lazyLoader.Load(this, ref ingredients); set => ingredients = value; }
        public Recipe() { }

        public Recipe(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
        public Recipe(string title, KitchenType kitchenType, string description, string[] instructions, double rating, User creator, int imageObjId, List<Ingredient> ingredients, int numberOfPeople)
        {
            Title = title;
            KitchenType = kitchenType;
            KitchenTypeId = kitchenType.Id;
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