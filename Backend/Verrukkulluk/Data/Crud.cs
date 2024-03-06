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
            return Context.Products.Include(p => p.ProductAllergies).ThenInclude(pa => pa.Allergy).FirstOrDefault(p => p.Id == id);
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

        public ImageObj ReadImageById(int Id)
        {
            return Context.ImageObjs.Where(i => i.Id == Id).First();
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

        public int? ReadUserRating(int recipeId, int userId)
        {
            var rating = Context.RecipeRatings
                .FirstOrDefault(r => r.RecipeId == recipeId && r.UserId == userId);

            return rating?.RatingValue;
        }

        public string? ReadUserComment(int recipeId, int userId)
        {
            var comment = Context.RecipeRatings
                .FirstOrDefault(c => c.RecipeId == recipeId && c.UserId == userId);

            return comment?.Comment;
        }

        public List<RecipeRating> ReadRatingsByUserId(int userId)
        {
            return Context.RecipeRatings.Where(c => c.UserId == userId).ToList();
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

        public void DeletePicture(int id)
        {
            ImageObj img = ReadImageById(id);
            if (img != null)
            {
                Context.ImageObjs.Remove(img);
            }
            Context.SaveChanges();
        }


        //public void AddParticipantToEvent(EventParticipant newParticipant, int id)
        //{
        //    //Add the participant to the event

        //    var eventModel = Context.Events.Include(e => e.Participants).Where(e => e.Id == id).FirstOrDefault();

        //    eventModel.Participants.Add(newParticipant);

        //    Context.SaveChanges();
        // }

        public Event AddParticipantToEvent(string name, string email, int eventId)
        {
            Event eventModel = Context.Events.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId);

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





    }
}
