using Microsoft.Extensions.Options;
using MlSuite.App.DTO.Configuration;
using MlSuite.Domain;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MlSuite.App.DTO;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.App.Services;


public class AccountsService : ServiceBase
{
    private readonly TrilhaDbContext _context;
    private readonly AccountBaseDataService _accountDataService;
    private readonly GenericDataService<AccountBase> _genericAccountDataService;
    //private readonly EmailService _emailService;
    private readonly JwtUtils _jwtUtils;
    private readonly AppSettings _settings;

    private AuthenticateResponseDto? _authenticatedResponseDto;

    public AuthenticateResponseDto? Response
    {
        get
        {
            AuthenticateResponseDto? response = _authenticatedResponseDto;
            _authenticatedResponseDto = null;
            return response;
        }
    }

    public AccountsService(TrilhaDbContext context, IServiceProvider provider, IOptions<AppSettings> settings) : base(context)
    {
        _context = context;
        _accountDataService = provider.GetRequiredService<AccountBaseDataService>();
        _genericAccountDataService = provider.GetRequiredService<GenericDataService<AccountBase>>();
        //_emailService = provider.GetRequiredService<EmailService>();
        _jwtUtils = provider.GetRequiredService<JwtUtils>();
        _settings = settings.Value;
    }

    //public async Task<bool> PreRegisterAgent(string email, string name, Guid tenant)
    //{
    //	//Checks if the tenant is valid
    //	CustomerProfile? tenantProfile = await ValidateTenant(tenant);
    //	if (tenantProfile is null)
    //	{
    //		return ServiceError(Services.ServiceError.InvalidTenant);
    //	}

    //	//Checks if the email has been already registered
    //	AgentAccount? tentative = await _context.Set<AgentAccount>()
    //		.Include(x=>x.Tenant)
    //		.FirstOrDefaultAsync(x=>x.Email == email && x.Tenant.Uuid == tenant);

    //	if (tentative is not null)
    //	{
    //		return ServiceError(Services.ServiceError.EmailAlreadyRegistered);
    //	}

    //	//Creates a temporarily-named agent with the email provided
    //	AgentAccount newAgent = new(tenantProfile, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Role.Agent,
    //		name, email);
    //	//Generates a verification token so the user can Authenticate themselves.
    //	newAgent.VerificationToken = await GenerateVerificationToken();
    //	//Saves the agent to the database
    //	await _genericAccountDataService.AddOrUpdate(newAgent);
    //	//Sends an email to the new user with the url to authenticate themselves.
    //	await SendVerificationEmail(newAgent);

    //	return true;
    //}

    //public async Task<bool> VerifyAccountByToken(string token)
    //{
    //	//Checks if the pre-registered can be identified with the token provided.
    //	AccountBase? tentativeAccount = await _accountDataService.GetByVerificationToken(token);
    //	if (tentativeAccount is null)
    //	{
    //		return ServiceError(Services.ServiceError.InvalidToken);
    //	}

    //	//Updates the user with the new information, and revokes the verification token
    //	return true;
    //}

    public async Task<bool> AuthenticateAccount(AuthenticateRequestDto authenticateRequestDto, string ipAddress)
    {
        string username = authenticateRequestDto.Login.Split('@')[0];
        string customer = authenticateRequestDto.Login.Split('@')[1];

        //CustomerProfile? tentativeCustomer = await _context.Set<CustomerProfile>().FirstOrDefaultAsync(x => x.CompanyIdentifier == customer);
        //if (tentativeCustomer is null)
        //{
        //    return ServiceError(Services.ServiceError.InvalidTenant);
        //}

        AccountBase? tentativeAgent = await _context.Set<AccountBase>()
            //.Include(x => x.Tenant)
            .FirstOrDefaultAsync(x => x.Username == username
                                      //&& x.Tenant.Uuid == tentativeCustomer.Uuid
                                      );

        if (tentativeAgent?.VerifiedAt is null || !BCrypt.Net.BCrypt.Verify(authenticateRequestDto.Password, tentativeAgent.Password))
        {
            return ServiceError(Services.ServiceError.WrongUsernameOrPassword);
        }

        DateTime jwtExpiry = DateTime.UtcNow.AddHours(12);
        var jwToken = _jwtUtils.GenerateJwt(tentativeAgent, jwtExpiry);
        RefreshToken refreshToken = await _jwtUtils.GenerateRefreshToken(ipAddress);
        tentativeAgent.RefreshTokens.Add(refreshToken);

        RemoveOldRefreshTokens(tentativeAgent);

        _context.Set<AccountBase>().Update(tentativeAgent);
        await _context.SaveChangesAsync();

        _authenticatedResponseDto = new AuthenticateResponseDto
        {
            Token = jwToken,
            RefreshToken = refreshToken.Token,
            Name = tentativeAgent.Name,
            Email = tentativeAgent.Email,
            Username = tentativeAgent.Username,
            ExpiryOn = jwtExpiry
        };

        return true;
    }

    public async Task<bool> RefreshRt(string token, string ipAddress)
    {
        AccountBase? tentativeAccount = await _accountDataService.GetByRefreshToken(token);
        if (tentativeAccount is null)
        {
            return ServiceError(Services.ServiceError.InvalidToken);
        }

        RefreshToken refreshToken = tentativeAccount.RefreshTokens.Single(x => x.Token == token);
        if (refreshToken.IsRevoked)
        {
            RevokeAllRefreshTokens(refreshToken, tentativeAccount, ipAddress, "Attempted reuse of revoked token");
            await _genericAccountDataService.AddOrUpdate(tentativeAccount);
        }

        if (!refreshToken.IsActive)
        {
            return ServiceError(Services.ServiceError.InvalidToken);
        }

        RefreshToken newRefreshToken = await RotateRefreshToken(refreshToken, ipAddress);
        tentativeAccount.RefreshTokens.Add(newRefreshToken);

        RemoveOldRefreshTokens(tentativeAccount);
        await _genericAccountDataService.AddOrUpdate(tentativeAccount);

        DateTime jwtExpiry = DateTime.UtcNow.AddHours(12);
        var jwToken = _jwtUtils.GenerateJwt(tentativeAccount, jwtExpiry);
        _authenticatedResponseDto = new AuthenticateResponseDto
        {
            Token = jwToken,
            RefreshToken = refreshToken.Token,
            Name = tentativeAccount.Name,
            Email = tentativeAccount.Email,
            Username = tentativeAccount.Username,
            ExpiryOn = jwtExpiry
        };
        return true;
    }

    private async Task<string> GenerateVerificationToken()
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        while (await _accountDataService.IsTokenUnique(token) == false)
        {
            token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        return token;
    }
    //private async Task SendVerificationEmail(AccountBase newAccount)
    //{
    //	string message = newAccount switch
    //	{
    //		AgentAccount newAgent => $@"<p>Você está recebendo esse email pois ele foi informado para o cadastro de agente da empresa {newAgent.Tenant.CorporateName}. Para aceitar, <a href=""http//:localhost/v1/auth/validateEmail?token={newAgent.VerificationToken}"">clique aqui</a>",
    //		UserAccount newUser => "",
    //		_ => throw new ArgumentOutOfRangeException()
    //	};

    //	await _emailService.SendAsync(
    //		to: newAccount.Email,
    //		subject: $"Nova conta criada - {newAccount.Tenant.CorporateName}",
    //		html: message);
    //}

    private void RemoveOldRefreshTokens(AccountBase account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.CreatedAt.AddDays(_settings.RefreshTokenTtl) <= DateTime.UtcNow);
    }

    private void RevokeAllRefreshTokens(RefreshToken refreshToken, AccountBase account, string ipAddress,
        string reason)
    {
        if (!string.IsNullOrWhiteSpace(refreshToken.ReplacedByToken))
        {
            RefreshToken? childToken =
                account.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken != null)
            {
                if (childToken.IsActive)
                {
                    RevokeRefreshToken(childToken, ipAddress, reason);
                }
                else
                {
                    RevokeAllRefreshTokens(childToken, account, ipAddress, reason);
                }
            }
        }
    }
    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokerIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    private async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        RefreshToken newRefreshToken = await _jwtUtils.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
}