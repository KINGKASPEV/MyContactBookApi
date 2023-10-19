using AutoMapper;
using ContactBookAPI.Model.DTOs;
using ContactBookAPI.Model;

namespace ContactBookAPI.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserToReturnDTO>();
            CreateMap<UserToReturnDTO, AppUser>();
        }
    }
}
