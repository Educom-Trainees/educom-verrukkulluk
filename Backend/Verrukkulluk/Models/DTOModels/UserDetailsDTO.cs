namespace Verrukkulluk.Models.DTOModels
{
    public class UserDetailsDTO
    {
        public string UserFirstName { get; set; }
        public string UserEmail { get; set; }
        public string UserCityOfResidence { get; set; }
        public string UserPhoneNumber { get; set; }

        /// <summary>
        /// Recipes Written by this user
        /// </summary>
        public List<RecipeBaseDTO> Recipes { get; set; }

        /// <summary>
        /// Favourite recipes of this user
        /// </summary>
        public List<RecipeBaseDTO> FavouriteRecipes { get; set; }

        /// <summary>
        /// Recipes this user has commented on
        /// </summary>
        public List<RecipeBaseDTO> CommentedRecipe {  get; set; }
    }
}
