
using Microsoft.AspNetCore.Mvc;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Data
{
        public interface ICrud
        {
                List<Product> ReadAllProducts();
                bool DeleteUserRecipe(int userId, int recipeId);
                List<RecipeInfo> ReadAllRecipes();
                List<RecipeInfo> ReadAllRecipesByUserId(int userId);
                Product? ReadProductById(int id);
                Product? ReadProductByName(string name);
                Recipe? ReadRecipeById(int id);
                RecipeInfo? ReadRecipeInfoById(int Id);
                ImageObj? ReadImageById(int Id);
                Event ReadEventById(int Id);
                List<Event> ReadAllEvents();
                bool AddOrUpdateRecipeRating(int recipeId, int? userId, int ratingValue, string? comment);

                List<RecipeRating> ReadAllRatings();

                RecipeRating? ReadRatingByUserIdAndRecipeId(int recipeId, int userId);
                List<RecipeRating> ReadRatingsByUserId(int userId);

                List<RecipeRating> ReadRatingsByRecipeId(int recipeId);

                bool DeleteRecipeRating(int recipeId, int userId);
                void UpdateAverageRating(int recipeId);
                void CreatePicture(ImageObj image);
                List<ImageObjInfo> ReadAllIPictureIds();
                bool DoesPictureExist(int imageObjId);
                bool IsPictureAvailable(int imageObjId, EImageObjType type, int targetId);
                void UpdatePicture(ImageObj image);
                void DeletePicture(int id);
                void CreateProduct(Product product);
                void UpdateProduct(Product product);
                void CreateRecipe(Recipe newRecipe);
                bool DoesRecipeTitleAlreadyExist(string title, int id);
                void UpdateRecipe(Recipe recipe);
                bool DeleteRecipe(int recipeId);
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
                bool DoesProductNameAlreadyExist(string name, int id);
                int ReadImageObjIdForProductId(int id);
                int ReadImageObjIdForAllergyId(int id);
                int ReadImageObjIdForRecipeId(int id);
                KitchenType? ReadKitchenTypeById(int kitchenTypeId);
                IEnumerable<KitchenType> ReadAllKitchenTypes();
                void CreateKitchenType(KitchenType kickenType);
                void UpdateKitchenType(KitchenType kickenType);
                bool DoesKitchenTypeExist(int id);
                bool DoesKitchenTypeNameAlreadyExist(string name, int id);
                bool DoesProductIdExists(int productId);
    }
}