using AutoMapper;
using Verrukkulluk.Models.DbModels;
using Verrukkulluk.Models.DTOModels;


namespace Verrukkulluk
{

    public class AutoMapper : Profile
    {
        public AutoMapper() {
            CreateMap<RecipeDTO, Recipe>();
            CreateMap<Recipe, RecipeDTO>();

            CreateMap<Product, ProductDTO>()
               .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(p => p.Allergy.Name).ToList()))
               .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(p => p.Product.Name).ToList()))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round(src.Price, 2)));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(name => new ProductAllergy { Allergy = new Allergy { Name = name } }).ToList()))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(name => new Ingredient { Product = new Product { Name = name } }).ToList()));
        } 
        
    }
}
