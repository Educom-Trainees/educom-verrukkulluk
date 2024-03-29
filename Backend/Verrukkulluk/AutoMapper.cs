﻿using AutoMapper;
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
                .ForMember(dest => dest.FavouriteRecipes,
                           opt => opt.MapFrom(src => src.User.FavouritesList.ToList()));

            CreateMap<RecipeRating, CommentDTO>();
            CreateMap<CommentDTO, RecipeRating>();

            CreateMap<RecipeRating, RecipeBaseDTO>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.Title,
                           opt => opt.MapFrom(src => src.Recipe.Title));
;

            CreateMap<UserDetailsDTO, UserDetailsModel>();

            CreateMap<RecipeDTO, Recipe>();
            CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.CreatorName,
                           opt => opt.MapFrom(src => src.Creator.FirstName));

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
