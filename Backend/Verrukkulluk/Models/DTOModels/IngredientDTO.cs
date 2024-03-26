using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOModels
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Range(0.01, 50_000.0, ErrorMessage = "Vul een aantal tussen 1 en 50.000 in")]
        public double Amount { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public IngredientType? ProductIngredientType { get; set; }
    }
}
