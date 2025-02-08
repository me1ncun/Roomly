using AutoMapper;
using Roomly.Shared.Data.Entities;
using Roomly.Users.ViewModels;

namespace Roomly.Users.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserViewModel, User>();
    }
}