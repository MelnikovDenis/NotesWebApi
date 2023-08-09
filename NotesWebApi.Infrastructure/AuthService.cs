using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using System.Net;
using System.Security.Claims;

namespace NotesWebApi.Infrastructure;

public class AuthService : IAuthService
{
   
    private IPasswordService PasswordService { get; }
    private ITokenService TokenService { get; }
    private ApplicationContext Context { get; }
    public AuthService(ApplicationContext context, IPasswordService passwordService, ITokenService tokenService)
    {
        Context = context;
        PasswordService = passwordService;
        TokenService = tokenService;
    }
    public async Task<UserInfoDto> RegisterAsync(UserRegisterDto userDto) 
    {
        var user = userDto.ToEntity(PasswordService);
        if (await Context.Users.FirstOrDefaultAsync(u => u.Email == user.Email) != null)
            throw new HttpRequestException("User with this email already exists.", null, HttpStatusCode.BadRequest);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return user.ToDto();
    }    
    public async Task<string> LoginAsync(UserLoginDto userDto, IResponseCookies cookies)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email.ToLower().Normalize())
            ?? throw new HttpRequestException("User with this email does not exist.", null, HttpStatusCode.Unauthorized);
        
        if (!PasswordService.VerifyPasswordHash(user.PasswordHash, userDto.Password))
            throw new HttpRequestException("Incorrect password.", null, HttpStatusCode.Unauthorized);

        RefreshToken refreshToken = TokenService.CreateRefreshToken(user);
        await TokenService.SetRefreshTokenAsync(refreshToken, cookies);
        await TokenService.DeleteExpiredTokensAsync(user);

        return TokenService.CreateAccessToken(user);
    }
    public async Task<string> RefreshTokenAsync(IRequestCookieCollection cookies) 
    {
        var refreshToken = await Context.RefreshTokens
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(t => t.Token == GetRefreshTokenFromCookies(cookies))
                    ?? throw new HttpRequestException("Incorrect refresh token.", null, HttpStatusCode.Unauthorized);
        if (refreshToken.Expires < DateTime.UtcNow)
            throw new HttpRequestException("Refresh token expired.", null, HttpStatusCode.Unauthorized);
        return TokenService.CreateAccessToken(refreshToken.Owner);
    }
    
    public async Task AllLogoutAsync(ClaimsPrincipal principal)
    {
        await Context.RefreshTokens
            .Where(t => t.Owner.Email == GetEmailFromClaimPrincipal(principal))
            .ExecuteDeleteAsync();
    }
    public async Task LogoutAsync(IRequestCookieCollection cookies)
    {
        await Context.RefreshTokens.Where(t => t.Token == GetRefreshTokenFromCookies(cookies)).ExecuteDeleteAsync();
    }
    private static string GetEmailFromClaimPrincipal(ClaimsPrincipal principal)
    {
        var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)
            ?? throw new HttpRequestException("Where is your email claim?", null, HttpStatusCode.Unauthorized);
        return claim.Value;
    }
    private static Guid GetRefreshTokenFromCookies(IRequestCookieCollection cookies) 
    {
        if (cookies.TryGetValue("RefreshToken", out string? refreshTokenStr)) 
        {
            if (Guid.TryParse(refreshTokenStr, out Guid token)) 
            {
                return token;
            }
            else 
            {
                throw new HttpRequestException("Incorrect refresh token.", null, HttpStatusCode.Unauthorized);
            }
        }
        else 
        {
            throw new HttpRequestException("Where is your refresh token?", null, HttpStatusCode.Unauthorized);
        }            
    }
   
}
