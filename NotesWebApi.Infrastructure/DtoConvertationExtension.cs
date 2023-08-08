using NotesWebApi.Domains.Entities;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Infrastructure;

public static class DtoConvertationExtension
{
    public static User ToEntity(this UserRegisterDto dto, IPasswordService passwordService) 
    {
        return new User 
        { 
            Nickname = dto.Nickname, 
            Email = dto.Email.ToLower().Normalize(), 
            PasswordHash = passwordService.CreatePasswordHash(dto.Password),
        };
    }
    public static UserInfoDto ToDto(this User entity) 
    {
        return new UserInfoDto 
        {
            Nickname = entity.Nickname, 
            Email = entity.Email 
        };
    }
}
