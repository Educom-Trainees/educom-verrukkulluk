namespace Verrukkulluk.Models.DTOModels
{
    public class UserDetailsDTO
    {
        public string UserFirstName { get; set; }
        public string UserEmail { get; set; }
        public string UserCityOfResidence { get; set; }
        public string UserPhoneNumber { get; set; }

        public List<string> FavouriteRecipesTitles { get; set; }

        public List<string> CommentedRecipe {  get; set; }

        public List<string> RecipeRatComment { get; set; }

    }
}
