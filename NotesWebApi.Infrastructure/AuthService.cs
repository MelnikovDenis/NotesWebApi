using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NotesWebApi.Infrastructure;

public class AuthService : IAuthService
{
    private IConfiguration Configuration { get; }
    private IPasswordService PasswordService { get; }
    private ApplicationContext Context { get; }
    public AuthService(IConfiguration configuration, ApplicationContext context, IPasswordService passwordService)
    {
        Configuration = configuration;
        Context = context;
        PasswordService = passwordService;
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
   
}
