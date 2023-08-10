using NotesWebApi.Domains.Entities;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Services.DtoConverters;

public interface IUserDtoConverter
{
    public User ToUser(UserRegisterDto userDto);
    public Task<User> ToUser(UserLoginDto userDto);
    public UserInfoDto ToUserInfoDto(User entity);
}
