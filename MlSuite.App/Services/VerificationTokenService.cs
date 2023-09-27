using System.Security.Cryptography;

namespace MlSuite.App.Services;

public class VerificationTokenService
{
    private readonly AccountBaseDataService _accountBaseDataService;

    public VerificationTokenService(IServiceProvider provider)
    {
        _accountBaseDataService = provider.GetRequiredService<AccountBaseDataService>();
    }

    public async Task<string> GenerateVerificationToken()
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        while (await _accountBaseDataService.IsTokenUnique(token) == false)
        {
            token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        return token;
    }
}