using AutoMapper;
using DotNetAssistant.Entities;
using DotNetAssistant.ViewModels;

namespace DotNetAssistant.Mapping;

public class ViewModelToEntityMappingProfile : Profile
{
    public ViewModelToEntityMappingProfile()
    {
        CreateMap<RegistrationViewModel, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
    }
}