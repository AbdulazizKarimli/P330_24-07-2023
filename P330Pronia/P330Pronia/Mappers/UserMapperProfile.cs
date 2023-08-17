using AutoMapper;
using P330Pronia.Models.Identity;
using P330Pronia.ViewModels;

namespace P330Pronia.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterViewModel, AppUser>().ReverseMap();
    }
}
