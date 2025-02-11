using AutoMapper;
using PhoneBookApi.Models;
using PhoneBookApi.ModelsDto;

namespace PhoneBookApi.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Настройка маппинга от сущности User к DTO UserDto
            CreateMap<User, UserDto>();

            // Если нужно и обратное преобразование:
            CreateMap<UserDto, User>();
        }
    }
}