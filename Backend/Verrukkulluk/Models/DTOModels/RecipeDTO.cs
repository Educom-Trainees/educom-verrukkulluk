using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Verrukkulluk.Models.DTOModels;

namespace Verrukkulluk.Models.DTOModels
{
    public class RecipeDTO
    {

        public int Id { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Titel is verplicht.")]
        public string Title { get; set; }
        [MaxLength(500)]
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Type keuken is verplicht.")]
        public int KitchenTypeId { get; set; }
     
        public string KitchenTypeName { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public string Price { get; set; }

        public int Calories { get; set; }

        [MaxLength(1000)]
        [Required(ErrorMessage = "Beschrijf tenminste stap 1.")]
        public string[] Instructions { get; set; }

        public DateOnly CreationDate { get; set; }

        public int CreatorId { get; set; }
        [ValidateNever]
        public User Creator { get; set; }

        public int NumberOfPeople { get; set; } = 4;

    }
}
