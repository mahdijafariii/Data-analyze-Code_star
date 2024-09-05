using AnalysisData.Data;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.PasswordResetTokensRepository.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.User.Repository.PasswordResetTokensRepository;

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

    public async Task<PasswordResetToken> GetToken(Guid guid)
    {
        return await _context.Tokens.Include(x => x.User).OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(x => x.UserId == guid);
    }
}