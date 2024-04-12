using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public double Calories { get; set; }
        [Required]
        [Range(1,20_000)]
        public double Amount { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A valid Image Id")]
        public int ImageObjId { get; set; }
        [Required]
        [Range(1, 20_000)]
        public double SmallestAmount { get; set; }
        [Required]
        public int PackagingId { get; set; }
        public string? PackagingName { get; set; }
        [Required]
        public IngredientType IngredientType { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool? InUse { get; set; }
        public List<AllergyDTO> Allergies { get; set; } = new List<AllergyDTO>();

    }
}
