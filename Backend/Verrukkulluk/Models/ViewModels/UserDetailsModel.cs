namespace Verrukkulluk.Models.ViewModels {
    public class UserDetailsModel {
        public User User { get; set; }
        public RecipeInfo[] Recipes { get; set; }
        public RecipeRating[] RecipeRatings { get; set;}
        //favorites to be implemented
    }
}
