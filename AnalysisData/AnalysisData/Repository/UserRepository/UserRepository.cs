using AnalysisData.Data;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string userName)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Username == userName);
            return user;
        }


        public async Task<IReadOnlyList<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeleteUser(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async void AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}