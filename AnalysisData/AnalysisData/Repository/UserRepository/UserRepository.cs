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

        public User GetUserByUsername(string userName)
        {
            return _context.Users.FirstOrDefault(x => x.Username == userName);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }


        public User GetUserById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }


        public async Task<List<User>> GetAllUserPagination(int page , int limit)
        {
            return await _context.Users.Skip((page - 1)*limit).Take(limit).ToListAsync();
        }
        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = GetUserById(id);
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
            var user = GetUserById(id);
            newUser.Id = user.Id;
            _context.Users.Update(newUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}