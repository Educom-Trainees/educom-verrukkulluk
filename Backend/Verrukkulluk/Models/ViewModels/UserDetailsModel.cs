using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Verrukkulluk.Models.ViewModels {
    public class UserDetailsModel {
        public User User { get; set; }
        [ValidateNever]
        public RecipeInfo[] Recipes { get; set; }
        [ValidateNever]
        public RecipeRating[] RecipeRatings { get; set;}
        //favorites to be implemented
    }
}
