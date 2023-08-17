using AutoMapper;
using P330Pronia.Areas.Admin.ViewModels.ProductViewModels;
using P330Pronia.Models;
using P330Pronia.ViewModels;

namespace P330Pronia.Mappers
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<CreateProductViewModel, Product>().ReverseMap();
            CreateMap<Product, ProductDetailViewModel>()
                .ForMember(pvm => pvm.TagNames, x => x.MapFrom(p => p.ProductTags.Select(pt => pt.Tag.Name)))
                .ReverseMap();
            CreateMap<Product, UpdateProductViewModel>()
                .ForMember(pvm => pvm.Image, x => x.Ignore())
                .ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
