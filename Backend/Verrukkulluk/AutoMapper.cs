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

            CreateMap<Allergy, AllergyDTO>();
            CreateMap<Product, ProductDTO>()
               .ForMember(dest => dest.Allergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(p => p.Allergy).ToList()))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round(src.Price, 2)));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.Allergies.Select(allergy => new ProductAllergy { ProductId = src.Id, AllergyId = allergy.Id }).ToList()));
                


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


            CreateMap<RecipeRating, CommentDTO>();
            CreateMap<CommentDTO, RecipeRating>();


            CreateMap<RecipeDTO, Recipe>();
            CreateMap<Recipe, RecipeDTO>();

        }

    }
}
