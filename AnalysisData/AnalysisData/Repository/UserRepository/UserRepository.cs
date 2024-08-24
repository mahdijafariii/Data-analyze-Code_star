using AnalysisData.Data;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.UserManage.Model;
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

        public async Task<User> GetUserByUsername(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == userName);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }


        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<User>> GetAllUserPagination(int page, int limit)
        {
            return await _context.Users.Skip((page)*limit).Take(limit).ToListAsync();
        }
        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user =await GetUserById(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(Guid id, User newUser)
        {
            var user =await GetUserById(id);
            newUser.Id = user.Id;
            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}