using System.ComponentModel.DataAnnotations;

namespace Verrukkulluk.Models
{
    public class RecipeRating
    {
        [Key]
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
