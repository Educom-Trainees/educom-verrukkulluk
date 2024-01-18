using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Verrukkulluk
{
    public class Recipe
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }         
        public List<DishType> DishType { get; set; } 
        public KitchenType KitchenType { get; set; }
        [MaxLength(1000)]
        public string Instructions { get; set; }
        virtual public ICollection<Comment> Comments { get; set; }
        public int Rating { get; set; }
        public DateOnly CreationDate { get; set; }
        public int UserId { get; set; }
        public IdentityUser Creator { get; set; }
        public string PhotoLocation { get; set; }

        public ICollection<Ingredient> Ingredient { get; set; }
    }     
}  
   



   