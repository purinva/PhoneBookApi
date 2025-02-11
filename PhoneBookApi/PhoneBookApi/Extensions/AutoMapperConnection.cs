using PhoneBookApi.Mappings;

namespace PhoneBookApi.Extensions
{
    public static class AutoMapperConnection
    {
        public static void AddAutoMapper(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(
                typeof(UserProfile)); // Добавляем профиль для маппинга
        }
    }
}