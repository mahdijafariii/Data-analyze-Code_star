using AnalysisData.Data;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.PasswordResetTokensRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.Repositories.PasswordResetTokensRepository;

public class PasswordResetTokensRepository : IPasswordResetTokensRepository
{
    private readonly ApplicationDbContext _context;

    public PasswordResetTokensRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddToken(PasswordResetToken token)
    {
        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync(); 
    }

    public async Task<PasswordResetToken> GetToken(Guid userId, string token)
    {
        return await _context.Tokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token);
    }
    
    public async Task SaveChange()
    {
        await _context.SaveChangesAsync();
    }
}