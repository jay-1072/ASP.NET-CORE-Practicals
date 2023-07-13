using AutoMapper;
using Practical19_View.Models;
using Practical19_View.ViewModels;

namespace Practical19_View.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, RegisterModel>();
        CreateMap<LoginViewModel, LoginModel>();
    }
}
