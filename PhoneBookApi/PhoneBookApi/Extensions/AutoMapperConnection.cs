using PhoneBookApi.Mappings;

namespace PhoneBookApi.Extensions
{
    public static class AutoMapperConnection
    {
        public static void AddAutoMapper(
            this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(UserProfile)); // Добавляем профиль для маппинга
        }
    }
}