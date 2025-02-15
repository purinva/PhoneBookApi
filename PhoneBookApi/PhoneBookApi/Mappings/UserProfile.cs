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

            // Для обратного преобразования
            CreateMap<UserDto, User>();
        }
    }
}