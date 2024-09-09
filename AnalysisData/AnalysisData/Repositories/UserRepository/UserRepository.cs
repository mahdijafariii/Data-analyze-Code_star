using AnalysisData.Data;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.UserRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Username == userName);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Email == email);
        }


        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<User>> GetAllUserPaginationAsync(int page, int limit)
        {
            return await _context.Users.Include(u => u.Role).Skip((page) * limit).Take(limit).ToListAsync();
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(Guid id, User newUser)
        {
            var user = await GetUserByIdAsync(id);
            newUser.Id = user.Id;
            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetTopUsersByUsernameSearchAsync(string username)
        {
            return await _context.Users.Include(u => u.Role)
                .Where(x => x.Username.Contains(username) && x.Role.RoleName != "data-analyst").Take(10).ToListAsync();
        }
    }
}