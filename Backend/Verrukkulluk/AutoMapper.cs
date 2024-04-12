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
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round(src.Price, 2)))
                .ForMember(dest => dest.InUse, opt => opt.MapFrom(src => src.Ingredients.Any()));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.Allergies.Select(allergy => new ProductAllergy { ProductId = src.Id, AllergyId = allergy.Id }).ToList()));



            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
            CreateMap<EventParticipant, ParticipantDTO>();
            CreateMap<ParticipantDTO, EventParticipant>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserDetailsModel, UserDetailsDTO>()
                .ForMember(dest => dest.FavouriteRecipes,
                           opt => opt.MapFrom(src => src.User.FavouritesList));

            CreateMap<RecipeRating, CommentDTO>();
            CreateMap<CommentDTO, RecipeRating>();

            CreateMap<RecipeRating, RecipeBaseDTO>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Title,
                           opt => opt.MapFrom(src => src.Recipe.Title));
;

            CreateMap<UserDetailsDTO, UserDetailsModel>();

            CreateMap<RecipeDTO, Recipe>()
                .ForMember(dest => dest.Ratings,
                           opt => opt.MapFrom(src => src.Comments));
            CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.CreatorName,
                           opt => opt.MapFrom(src => src.Creator.FirstName))
                .ForMember(dest => dest.Comments,
                           opt => opt.MapFrom(src => src.Ratings));

            CreateMap<Recipe, RecipeBaseDTO>();
            CreateMap<RecipeRating, RecipeBaseDTO>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Comments,
                            opt => opt.MapFrom(src => src.Comment));

            CreateMap<IngredientDTO, Ingredient>();
            CreateMap<Ingredient, IngredientDTO>();
        }
    }
}
