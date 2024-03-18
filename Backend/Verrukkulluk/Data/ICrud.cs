
using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Data
{
        public interface ICrud
        {
                List<Product> ReadAllProducts();
                string DeleteUserRecipe(int userId, int recipeId);
                List<RecipeInfo>? ReadAllRecipes();
                List<RecipeInfo>? ReadAllRecipesByUserId(int userId);
                Product? ReadProductById(int id);
                Product? ReadProductByName(string name);
                public RecipeInfo ReadRecipeById(int Id);
                public ImageObj? ReadImageById(int Id);
                public Event ReadEventById(int Id);
                List<Event> ReadAllEvents();
                bool AddOrUpdateRecipeRating(int recipeId, int? userId, int ratingValue, string? comment);

                List<RecipeRating> ReadAllRatings();
                int? ReadUserRating(int recipeId, int userId);
                string? ReadUserComment(int recipeId, int userId);
                List<RecipeRating> ReadRatingsByUserId(int userId);
                void UpdateAverageRating(int recipeId);
                void CreatePicture(ImageObj image);
                List<ImageObjInfo> ReadAllIPictureIds();
                bool DoesPictureExist(int imageObjId);
                bool IsPictureAvailiable(int imageObjId, EImageObjType type, int targetId);
                void UpdatePicture(ImageObj image);
                void DeletePicture(int id);
                Product CreateProduct(Product product);
                void UpdateProduct(Product product);
                void CreateRecipe(Recipe newRecipe);
                void UpdateRecipe(Recipe recipe);
                string DeleteRecipe(int recipeId);
                Event AddParticipantToEvent(string name, string email, int id);
                List<Allergy> ReadAllAllergies();
                Allergy? ReadAllergyById(int id);
                void CreateAllergy(Allergy allergy);
                bool DoesAllergyNameAlreadyExist(string name, int id);
                bool DoAllergiesExist(int[] ids);
                void UpdateAllergy(Allergy allergy);
                List<PackagingType> ReadAllPackagingTypes();
                bool DoesPackagingTypeExist(int id);
                void CreatePackagingType(PackagingType packagingType);
                void UpdatePackagingType(PackagingType packagingType);
                bool DoesPackagingTypeNameAlreadyExist(string name, int id);
    }
}