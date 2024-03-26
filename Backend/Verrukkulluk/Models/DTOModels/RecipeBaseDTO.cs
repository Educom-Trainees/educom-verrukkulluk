using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models.DTOModels
{
    public class RecipeBaseDTO
    {

        public int Id { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Titel is verplicht.")]
        public string Title { get; set; }

        public List<CommentDTO>? Comments { get; set; }
    }
}
