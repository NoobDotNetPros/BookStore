using AutoMapper;
using Bookstore.Models.DTOs;
using Bookstore.Models.Entities;

namespace Bookstore.Business.Services.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
