using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Models;

namespace PhoneBookApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .UseIdentityColumn(); // Использует автоинкремент

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()  // Поле обязательное (не может быть null)
                .HasMaxLength(100);  // Максимальная длина строки для имени

            modelBuilder.Entity<User>()
                .Property(u => u.Surname)
                .IsRequired()  // Поле обязательное
                .HasMaxLength(100);  // Максимальная длина для фамилии

            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .IsRequired()  // Поле обязательное
                .HasMaxLength(16);  // Максимальная длина номера телефона

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)  // Индекс на поле PhoneNumber
                .IsUnique();  // Уникальный индекс, запрещает повторяющиеся номера

            base.OnModelCreating(modelBuilder);
        }
    }
}