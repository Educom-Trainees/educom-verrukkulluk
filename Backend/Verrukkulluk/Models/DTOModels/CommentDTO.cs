namespace Verrukkulluk.Models.DTOModels
{
    public class CommentDTO
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public string? Comment { get; set; }
        public int RatingValue { get; set; }
    }
}
