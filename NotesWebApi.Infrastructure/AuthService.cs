using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using System.Net;

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
    public async Task<string> LoginAsync(UserLoginDto userDto, HttpResponse httpResponse)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email.ToLower().Normalize())
            ?? throw new HttpRequestException("User with this email does not exist.", null, HttpStatusCode.Unauthorized);
        
        if (!PasswordService.VerifyPasswordHash(user.PasswordHash, userDto.Password))
            throw new HttpRequestException("Incorrect password.", null, HttpStatusCode.Unauthorized);

        RefreshToken refreshToken = TokenService.CreateRefreshToken(user);
        await TokenService.SetRefreshTokenAsync(refreshToken, httpResponse);
        await TokenService.DeleteExpiredTokensAsync(user);

        return TokenService.CreateAccessToken(user);
    }
}
