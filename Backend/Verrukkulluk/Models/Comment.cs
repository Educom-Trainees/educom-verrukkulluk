using Verrukkulluk.Models;

namespace Verrukkulluk
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; } 
        public Recipe Recipe  { get; set; }
        public string Content { get; set; }
    }
}