using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using NotesWebApi.Services.DtoConverters;
using System.Net;
using System.Security.Claims;

namespace NotesWebApi.Infrastructure;

public class AuthService : IAuthService
{
   
    private IPasswordService PasswordService { get; }
    private ITokenService TokenService { get; }
    private IUserDtoConverter UserDtoConverter { get; }
    private ApplicationContext Context { get; }
    public AuthService(ApplicationContext context, IPasswordService passwordService, ITokenService tokenService, IUserDtoConverter userDtoConverter)
    {
        Context = context;
        PasswordService = passwordService;
        TokenService = tokenService;
        UserDtoConverter = userDtoConverter;
    }
    public async Task<UserInfoDto> RegisterAsync(UserRegisterDto userDto) 
    {
        var user = UserDtoConverter.ToUser(userDto);
        if (await Context.Users.FirstOrDefaultAsync(u => u.Email == user.Email) != null)
            throw new HttpRequestException("User with this email already exists.", null, HttpStatusCode.BadRequest);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return UserDtoConverter.ToUserInfoDto(user);
    }
    public async Task<string> LoginAsync(UserLoginDto userDto)
    {
        var user = await UserDtoConverter.ToUser(userDto);
        
        if (!PasswordService.VerifyPasswordHash(user.PasswordHash, userDto.Password))
            throw new HttpRequestException("Incorrect password.", null, HttpStatusCode.Unauthorized);

        RefreshToken refreshToken = TokenService.CreateRefreshToken(user);
        await TokenService.SetRefreshTokenAsync(refreshToken);
        await TokenService.DeleteExpiredTokensAsync(user);

        return TokenService.CreateAccessToken(user);
    }
    public async Task<string> RefreshTokenAsync() 
    {
        var refreshToken = await Context.RefreshTokens
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(t => t.Token == TokenService.GetRefreshToken())
                    ?? throw new HttpRequestException("Incorrect refresh token.", null, HttpStatusCode.Unauthorized);
        if (refreshToken.Expires < DateTime.UtcNow)
            throw new HttpRequestException("Refresh token expired.", null, HttpStatusCode.Unauthorized);
        return TokenService.CreateAccessToken(refreshToken.Owner);
    }  
    public async Task AllLogoutAsync()
    {
        var id = (await TokenService.GetUserFromClaims()).Id;
        await Context.RefreshTokens
            .Where(t => t.Owner.Id == id)
            .ExecuteDeleteAsync();
    }
    public async Task LogoutAsync()
    {
        await Context.RefreshTokens.Where(t => t.Token == TokenService.GetRefreshToken()).ExecuteDeleteAsync();
    }
}
