using Microsoft.AspNetCore.Http;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Services;

public interface IAuthService
{    
    public Task<UserInfoDto> RegisterAsync(UserRegisterDto userDto);
    public Task<string> LoginAsync(UserLoginDto userDto, HttpResponse httpResponse);
}
