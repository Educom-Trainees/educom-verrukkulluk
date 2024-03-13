using AutoMapper;
using Verrukkulluk.Models.DbModels;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models;
using Verrukkulluk.ViewModels;
using Verrukkulluk.Models.ViewModels;


namespace Verrukkulluk
{

    public class AutoMapper : Profile
    {
        public AutoMapper() {

            CreateMap<Product, ProductDTO>()
               .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(p => p.Allergy.Name).ToList()))
               .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(p => p.Product.Name).ToList()))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round(src.Price, 2)));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(name => new ProductAllergy { Allergy = new Allergy { Name = name } }).ToList()))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(name => new Ingredient { Product = new Product { Name = name } }).ToList()));


            CreateMap<Event, EventDTO>()
                .ForMember(dest => dest.EventParticipantName,
                 opt => opt.MapFrom(src => src.Participants.Select(p => p.Name).ToList()));
            CreateMap<EventDTO, Event>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserDetailsModel, UserDetailsDTO>()
                .ForMember(dest => dest.FavouriteRecipesTitles,
                 opt => opt.MapFrom(src => src.User.FavouritesList.Select(recipe => recipe.Title).ToList()))
                .ForMember(dest => dest.CommentedRecipe,
                 opt => opt.MapFrom(src => src.RecipeRatings.Select(recipeRating => recipeRating.Recipe.Title).ToList()))
                .ForMember(dest => dest.RecipeComment,
                 opt => opt.MapFrom(src => src.RecipeRatings.Select(recipeRating => recipeRating.Comment).ToList()));

            CreateMap<UserDetailsDTO, UserDetailsModel>();


            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();


            CreateMap<RecipeDTO, Recipe>();
            CreateMap<Recipe, RecipeDTO>();

        }

    }
}
