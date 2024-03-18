using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOModels
{
    public class AllergyDTO
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ImgObjId { get; set; }
    }
}