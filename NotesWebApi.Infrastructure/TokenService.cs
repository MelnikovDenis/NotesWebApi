using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
namespace NotesWebApi.Infrastructure;

public class TokenService : ITokenService
{
    private IConfiguration Configuration { get; }
    private ApplicationContext Context { get; }
    public TokenService(IConfiguration configuration, ApplicationContext context)
    {
        Configuration = configuration;
        Context = context;
    }
    public string CreateAccessToken(User user)
    {
        var issuer = Configuration.GetSection("JwtSettings:Issuer").Value;
        var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };
        var expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(double.Parse(Configuration.GetSection("JwtSettings:AccessTokenExpires").Value!)));
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                Configuration.GetSection("JwtSettings:TokenKey").Value!
            )
        );
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(issuer, null, claims, null, expires, credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public RefreshToken CreateRefreshToken(User user)
    {
        var refreshToken = new RefreshToken {
            Token = Guid.NewGuid(),
            Expires = DateTime.UtcNow.Add(
                    TimeSpan.FromSeconds(double.Parse(Configuration.GetSection("JwtSettings:RefreshTokenExpires").Value!))
                ),
            Owner = user
        };
        return refreshToken;
    }
    public async Task SetRefreshTokenAsync(RefreshToken refreshToken, IResponseCookies cookies) 
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = refreshToken.Expires            
        };
        cookies.Append("RefreshToken", refreshToken.Token.ToString(), cookieOptions);
        Context.RefreshTokens.Add(refreshToken);
        await Context.SaveChangesAsync();
    }
    public async Task DeleteExpiredTokensAsync(User user) 
    {
        await Context.RefreshTokens
            .Include(t => t.Owner)
            .Where(t => t.Owner.Id == user.Id && t.Expires < DateTime.UtcNow)
            .ExecuteDeleteAsync();
    }    
}