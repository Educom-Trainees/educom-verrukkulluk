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
               .ForMember(dest => dest.ProductAllergies, opt => opt.MapFrom(src => src.ProductAllergies.Select(p => p.Allergy.Name).ToList()));
            CreateMap<ProductDTO, Product>();
        } 
        
    }
}
