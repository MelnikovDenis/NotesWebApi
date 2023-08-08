using NotesWebApi.Domains.Entities;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Services;

public interface IAuthService
{    
    public string CreateAccessToken(User user);
    public Task<UserInfoDto> RegisterAsync(UserRegisterDto user); 
}
