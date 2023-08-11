using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto.User;
using NotesWebApi.Services.DtoConverters;
using System.Net;

namespace NotesWebApi.Infrastructure.DtoConverters;

public class UserDtoConverter : IUserDtoConverter
{
    private IPasswordService PasswordService { get; }
    private ApplicationContext Context { get; }
    public UserDtoConverter(IPasswordService passwordService, ApplicationContext context)
    {
        PasswordService = passwordService;
        Context = context;
    }
    public User ToUser(UserRegisterDto userDto)
    {
        return new User
        {
            Nickname = userDto.Nickname,
            Email = userDto.Email.ToLower().Normalize(),
            PasswordHash = PasswordService.CreatePasswordHash(userDto.Password),
        };
    }
    public async Task<User> ToUser(UserLoginDto userDto)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email.ToLower().Normalize())
            ?? throw new HttpRequestException("User with this email does not exist.", null, HttpStatusCode.Unauthorized);
    }
    public UserInfoDto ToUserInfoDto(User entity)
    {
        return new UserInfoDto
        {
            Nickname = entity.Nickname,
            Email = entity.Email
        };
    }
}
