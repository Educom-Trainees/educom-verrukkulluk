using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Numerics;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DbModels;

namespace Verrukkulluk.Data
{
    public class Crud : ICrud
    {
        VerrukkullukContext Context;

        public Crud(VerrukkullukContext context)
        {
            Context = context;
        }

        public List<Product> ReadAllProducts()
        {
            return Context.Products
                .Include(p => p.Packaging)
                .Include(p => p.ProductAllergies)
                    .ThenInclude(pa => pa.Allergy)
                .ToList();
        }
        public Product? ReadProductById(int id)
        {
            return Context.Products.Include(p => p.Packaging).Include(p => p.ProductAllergies).ThenInclude(pa => pa.Allergy).FirstOrDefault(p => p.Id == id);
        }

        public Product? ReadProductByName(string name)
        {
            return Context.Products.Include(p => p.Packaging).Include(p => p.ProductAllergies).ThenInclude(pa => pa.Allergy).FirstOrDefault(p => p.Name == name);
        }

        public string DeleteUserRecipe(int userId, int recipeId)
        {
            try
            {
                var selectedRecipe = Context.Recipes
                    .Where(recipe => recipe.CreatorId == userId)
                    .FirstOrDefault(recipe => recipe.Id == recipeId);

                if (selectedRecipe != null)
                {
                    Context.Recipes.Remove(selectedRecipe);
                    Context.SaveChanges();
                    return "Recept verwijderd.";
                }
                else
                {
                    return "Recept kon niet worden gevonden.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return "Er ging iets mis. Probeer het later opnieuw";
            }
        }

        public string DeleteRecipe(int recipeId)
        {
            try
            {
                var selectedRecipe = Context.Recipes
                    .FirstOrDefault(recipe => recipe.Id == recipeId);

                if (selectedRecipe != null)
                {
                    Context.Recipes.Remove(selectedRecipe);
                    Context.SaveChanges();
                    return "Recept verwijderd.";
                }
                else
                {
                    return "Recept kon niet worden gevonden.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return "Er ging iets mis. Probeer het later opnieuw";
            }
        }


        public List<RecipeInfo>? ReadAllRecipes()
        {
            try
            {
                var recipes = Context.Recipes
                    .Include(r => r.KitchenType)
                    .Include(r => r.Creator)
                    .Include(r => r.Ingredients)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.ProductAllergies)
                                .ThenInclude(p => p.Allergy)
                    .OrderBy(recipe => recipe.CreationDate)
                    .Select(r => new RecipeInfo(r))
                    .ToList();
                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            return null;
        }

        public List<RecipeInfo>? ReadAllRecipesByUserId(int userId)
        {
            try
            {
                var recipes = Context.Recipes
                    .Include(r => r.KitchenType)
                    .Include(r => r.Creator)
                    .Include(r => r.Ingredients)
                        .ThenInclude(i => i.Product)
                            .ThenInclude(p => p.ProductAllergies)
                                .ThenInclude(p => p.Allergy)
                    .Where(recipe => recipe.CreatorId == userId)
                    .Select(r => new RecipeInfo(r))
                    .ToList();
                return recipes;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }

            return null;
        }

        public RecipeInfo ReadRecipeById(int Id)
        {
            var recipe = Context.Recipes
                .Include(r => r.KitchenType)
                .Include(r => r.Creator)
                .Include(r => r.Ingredients)
                    .ThenInclude(r => r.Product)
                        .ThenInclude(p => p.ProductAllergies)
                            .ThenInclude(p => p.Allergy)
                .FirstOrDefault(i => i.Id == Id);

            if (recipe != null)
            {
                recipe.KitchenTypeId = recipe.KitchenType.Id; // For some reason this is not handled by the [ForeignKey] annotation

                var recipeInfo = new RecipeInfo(recipe);

                var allRatings = Context.RecipeRatings
                    .Where(r => r.RecipeId == recipe.Id)
                    .ToList();

                string? userFirstName = null;

                foreach (var rating in allRatings)
                {
                    if (rating.UserId != null)
                    {
                        var user = Context.Users.FirstOrDefault(u => u.Id == rating.UserId);
                        if (user != null)
                        {
                            userFirstName = user.FirstName;
                        }
                    }
                    else
                    {
                        userFirstName = "Anonymous";
                    }
                    var recipeRating = new RecipeRating
                    {
                        Id = rating.Id,
                        RecipeId = rating.RecipeId,
                        UserId = rating.UserId,
                        RatingValue = rating.RatingValue,
                        Comment = rating.Comment,
                    };
                    recipeInfo.Ratings.Add(recipeRating);
                }
                return recipeInfo;
            }
            return null;
        }

        public ImageObj? ReadImageById(int Id)
        {
            return Context.ImageObjs.Where(i => i.Id == Id).FirstOrDefault();
        }
        public List<ImageObjInfo> ReadAllIPictureIds()
        {
            var ids = Context.ImageObjs.Select(i => i.Id);
            var allergyImageIds = Context.Allergies.Select(a => a.ImgObjId);
            var productImageIds = Context.Products.Select(p => p.ImageObjId);
            var receiptImageIds = Context.Recipes.Select(r => r.ImageObjId);
            var userImageIds = Context.Users.Select(u => u.ImageObjId);
            return ids.Select(id => new ImageObjInfo
            {
                Id = id,
                UsedBy =
                allergyImageIds.Contains(id) ? EImageObjType.Allergy :
                receiptImageIds.Contains(id) ? EImageObjType.Recipe :
                productImageIds.Contains(id) ? EImageObjType.Product :
                userImageIds.Contains(id) ?    EImageObjType.User : 
                                               EImageObjType.None
            }).ToList();

        }
        public bool DoesPictureExist(int id)
        {
            return Context.ImageObjs.Any(i => i.Id == id);
        }

        public bool IsPictureAvailiable(int imageObjId, EImageObjType type, int targetId)
        {
            if (targetId == 0) { type = EImageObjType.None; }

            var allergyImageId = Context.Allergies.Where(a => a.ImgObjId == imageObjId).Select(a => a.Id).FirstOrDefault();
            var productImageId = Context.Products.Where(p => p.ImageObjId == imageObjId).Select(a => a.Id).FirstOrDefault();
            var receiptImageId = Context.Recipes.Where(r => r.ImageObjId == imageObjId).Select(a => a.Id).FirstOrDefault();
            var userImageId = Context.Users.Where(u => u.ImageObjId == imageObjId).Select(a => a.Id).FirstOrDefault();

            switch (type)
            {
                case EImageObjType.Allergy: return (allergyImageId == 0 || allergyImageId == targetId) && productImageId == 0 && receiptImageId == 0 && userImageId == 0;
                case EImageObjType.Product: return allergyImageId == 0 && (productImageId == 0 || productImageId == targetId) && receiptImageId == 0 && userImageId == 0;
                case EImageObjType.Recipe: return allergyImageId == 0 && productImageId == 0 && (receiptImageId == 0 || receiptImageId == targetId) && userImageId == 0;
                case EImageObjType.User: return allergyImageId == 0 && productImageId == 0 && receiptImageId == 0 && (userImageId == 0 || userImageId == targetId);
                default: return allergyImageId == 0 && productImageId == 0 && receiptImageId == 0 && userImageId == 0;
            }

        }

        public Event ReadEventById(int Id)
        {
            return Context.Events.Include(e => e.Participants).Where(e => e.Id == Id).First();
        }

        public List<Event> ReadAllEvents()
        {
            return Context.Events.ToList();
        }

        public bool AddOrUpdateRecipeRating(int recipeId, int? userId, int ratingValue, string? comment)
        {
            try
            {
                if (userId == null)
                {
                    Context.RecipeRatings.Add(new RecipeRating
                    {
                        RecipeId = recipeId,
                        UserId = userId,
                        RatingValue = ratingValue,
                        Comment = comment
                    });
                }
                else
                {
                    var existingRating = Context.RecipeRatings.FirstOrDefault(r => r.RecipeId == recipeId && r.UserId == userId);

                    if (existingRating != null)
                    {
                        existingRating.RatingValue = ratingValue;
                        existingRating.Comment = comment;
                    }
                    else
                    {
                        Context.RecipeRatings.Add(new RecipeRating
                        {
                            RecipeId = recipeId,
                            UserId = userId,
                            RatingValue = ratingValue,
                            Comment = comment
                        });
                    }
                }
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return false;
            }
        }

        public List<RecipeRating> ReadRatingsByRecipeId(int recipeId)
        {
            return Context.RecipeRatings.Where(c => c.RecipeId == recipeId).ToList();
        }

        public List<RecipeRating> ReadRatingsByUserId(int userId)
        {
            return Context.RecipeRatings.Where(c => c.UserId == userId).ToList();
        }

      
        public RecipeRating ReadRatingByUserIdAndRecipeId(int recipeId, int userId)
        {

            var recipeRating = Context.RecipeRatings
                          .FirstOrDefault(c => c.RecipeId == recipeId && c.UserId == userId);

            if(recipeRating == null)
            {
                return null;
            }

            return recipeRating;
   
        }


        public List<RecipeRating> ReadAllRatings()
        {
            return Context.RecipeRatings.Select(c => c).ToList();
        }


        public double? GetAverageRating(int recipeId)
        {
            var ratings = Context.RecipeRatings.Where(r => r.RecipeId == recipeId).Select(r => r.RatingValue);
            if (ratings.Any())
            {
                return Math.Round(ratings.Average(), 2);
            }
            else
            {
                return 0;
            }
        }

        public void UpdateAverageRating(int recipeId)
        {
            var averageRating = GetAverageRating(recipeId);
            if (averageRating != null)
            {
                var recipe = Context.Recipes.Find(recipeId);
                recipe.AverageRating = (double)averageRating;
                Context.SaveChanges();
            }
        }
        public void CreatePicture(ImageObj image)
        {
            Context.ImageObjs.Add(image);
            Context.SaveChanges();
        }

        public Product CreateProduct(Product product)
        {
            try
            {
                if (product != null)
                {
                    Context.Products.Add(product);
                    Context.SaveChanges();

                    return product;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return null;
            }
        }

        public void UpdateProduct(Product product)
        {
            Context.Products.Update(product);
            Context.SaveChanges();
        }

        public void CreateRecipe(Recipe newRecipe)
        {
            Context.Recipes.Add(newRecipe);
            Context.SaveChanges();
        }

        public void UpdateRecipe(Recipe recipe)
        {
            Context.Recipes.Update(recipe);
            Context.SaveChanges();
        }

        public void UpdatePicture(ImageObj image)
        {
            Context.ImageObjs.Update(image);
            Context.SaveChanges();
        }
        public void DeletePicture(int id)
        {
            ImageObj? img = ReadImageById(id);
            if (img != null)
            {
                Context.ImageObjs.Remove(img);
            }
            Context.SaveChanges();
        }

        public Event AddParticipantToEvent(string name, string email, int eventId)
        {
            Event? eventModel = Context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);

            if (eventModel != null)
            {
                EventParticipant newParticipant = new EventParticipant { Name = name, Email = email };

                eventModel.Participants.Add(newParticipant);

                Context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Event with ID {eventId} not found.");
            }
            return eventModel;


        }

        public List<Allergy> ReadAllAllergies()
        {
            return Context.Allergies.ToList();
        }

        public Allergy? ReadAllergyById(int id)
        {
            return Context.Allergies.Find(id);
        }

        public void CreateAllergy(Allergy allergy)
        {
            Context.Allergies.Add(allergy);
            Context.SaveChanges();
        }

        public void UpdateAllergy(Allergy allergy)
        {
            Context.Allergies.Update(allergy);
            Context.SaveChanges();
        }


        public List<PackagingType> ReadAllPackagingTypes()
        {
            return Context.PackagingTypes.ToList();
        }

        public bool DoAllergiesExist(int[] ids)
        {
            return Context.Allergies.Where(a => ids.Contains(a.Id)).Count() == ids.Length;
        }

        public bool DoesPackagingTypeExist(int id)
        {
            return Context.PackagingTypes.Any(i => i.Id == id);
        }

        public bool DoesAllergyNameAlreadyExist(string name, int id)
        {
            return Context.Allergies.Any(a => a.Name == name && a.Id != id);
        }

        public void CreatePackagingType(PackagingType packagingType)
        {
            Context.PackagingTypes.Add(packagingType);
            Context.SaveChanges();
        }

        public void UpdatePackagingType(PackagingType packagingType)
        {
            Context.PackagingTypes.Update(packagingType);
            Context.SaveChanges();
        }

        public bool DoesPackagingTypeNameAlreadyExist(string name, int id)
        {
            return Context.PackagingTypes.Any(a => a.Name == name && a.Id != id);
        }

        
    }
}
