namespace Verrukkulluk.Models.DTOModels
{
    public class CommentDTO
    {
        public int? Id { get; set; }
        /// <summary>
        /// Optional the user that left the rating, if <code>null</code> it is an anonymous user
        /// </summary>
        public int? UserId { get; set; }
        public int RecipeId { get; set; }
        /// <summary>
        /// The comment part is optional
        /// </summary>
        public string? Comment { get; set; }
        public int RatingValue { get; set; }
    }
}
