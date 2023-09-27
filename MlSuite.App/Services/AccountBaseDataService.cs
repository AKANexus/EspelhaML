using Microsoft.EntityFrameworkCore;
using MlSuite.Domain.Enums;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.App.Services;


public class AccountBaseDataService
{
    private readonly TrilhaDbContext _context;

    public AccountBaseDataService(TrilhaDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsTokenUnique(string token)
    {
        return !(await _context.Set<AccountBase>().AnyAsync(x => x.VerificationToken == token) ||
                 await _context.Set<AccountBase>().AnyAsync(x => x.PasswordResetToken == token));
    }

    public async Task<bool> IsRefreshTokenUnique(string token)
    {
        return !await _context.Set<AccountBase>()
            .Include(x => x.RefreshTokens)
            .AnyAsync(x => x.RefreshTokens.Any(y => y.Token == token));
    }

    public async Task<AccountBase?> GetByUuidAndRole(Guid uuid, Role role)
    {
        return await _context.Set<AccountBase>()
            //.Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.Uuid == uuid && x.Role == role);
    }

    public async Task<AccountBase?> GetByUuidAndTenant(Guid uuid, Guid tenant)
    {
        return await _context.Set<AccountBase>()
            //.Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.Uuid == uuid
                                      //&& x.Tenant.Uuid == tenant
                                      );
    }

    public async Task<AccountBase?> GetByRefreshToken(string refreshToken)
    {
        return await _context.Set<AccountBase>().FirstOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken));
    }

    public async Task<AccountBase?> GetByVerificationToken(string verificationToken)
    {
        return await _context.Set<AccountBase>()
            //.Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.VerificationToken == verificationToken);
    }
}