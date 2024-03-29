﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Verrukkulluk.Models.DTOModels;

namespace Verrukkulluk.Models.DTOModels
{
    public class RecipeDTO : RecipeBaseDTO
    {
        [MaxLength(500)]
        [Required(ErrorMessage = "Beschrijving is verplicht.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Type keuken is verplicht.")]
        public int KitchenTypeId { get; set; }
     
        public string? KitchenTypeName { get; set; }

        [MaxLength(1000)]
        [Required(ErrorMessage = "Beschrijf tenminste stap 1.")]
        public string[] Instructions { get; set; }

        public DateOnly CreationDate { get; set; }

        public int CreatorId { get; set; }
        public string? CreatorName { get; set; }

        public int NumberOfPeople { get; set; } = 4;

        public int ImageObjId { get; set; }

        [Required(ErrorMessage = "Tenminste 1 ingredient")]
        public List<IngredientDTO> Ingredients { get; set; }
    }
}
