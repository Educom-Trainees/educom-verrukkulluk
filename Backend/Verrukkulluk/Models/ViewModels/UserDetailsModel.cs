namespace Verrukkulluk.Models.ViewModels {
    public class UserDetailsModel {
        public User User { get; set; }
        public List<RecipeInfo> Recipes { get; set; }
        public List<RecipeRating> RecipeRatings { get; set;}
        //favorites to be implemented
    }
}
