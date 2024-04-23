using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOmodels
{
    public class PackagingTypeDTO
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [MaxLength(255)]
        public string Name { get; set; } = "";
        public bool Active { get; set; }
        public bool? InUse { get; set; }
    }
}