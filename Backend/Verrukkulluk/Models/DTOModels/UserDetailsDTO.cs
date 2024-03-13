namespace Verrukkulluk.Models.DTOModels
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CityOfResidence { get; set; }
        public string PhoneNumber { get; set; }

        public List<string> FavouriteRecipesTitles { get; set; }

        public List<string> CommentedRecipe {  get; set; }

        public List<string> RecipeRatingsComment { get; set; }

    }
}
