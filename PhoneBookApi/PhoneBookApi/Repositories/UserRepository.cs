using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Data;
using PhoneBookApi.Models;

namespace PhoneBookApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            // Проверка на уникальность телефона
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Phone number is already taken.");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetPaginatedAsync(
            int page, int pageSize)
        {
            return await _context.Users
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}