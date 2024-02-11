using AutoMapper;
using budgetManager.Dto.UserDto;
using budgetManager.Models;

namespace budgetManager.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //User
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}
