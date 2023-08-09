using Microsoft.AspNetCore.Http;
using NotesWebApi.Services.Dto;
using System.Security.Claims;

namespace NotesWebApi.Services;

/// <summary>
///  Интерфейс сервиса для аутенфикации и авторизации пользователей
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///  Регистрация пользователя
    /// </summary>
    public Task<UserInfoDto> RegisterAsync(UserRegisterDto userDto);
    /// <summary>
    ///  Аутенфикация пользователя
    /// </summary>
    /// <param name="cookies">Необходим для установки в cookie refresh-токена</param>
    public Task<string> LoginAsync(UserLoginDto userDto, IResponseCookies cookies);
    /// <summary>
    ///  Получение нового access-токена
    /// </summary>
    /// <param name="cookies">Необходим для считывания refresh-токена</param>
    public Task<string> RefreshTokenAsync(IRequestCookieCollection cookies);
    /// <summary>
    ///  Удаление всех refresh-токенов пользователя из базы данных
    /// </summary>
    /// <param name="principal">Необходим для идентификации пользователя по его claim'ам</param>
    public Task AllLogoutAsync(ClaimsPrincipal principal);
    /// <summary>
    ///  Удаление текущего refresh-токена пользователя из базы данных
    /// </summary>
    /// <param name="cookies">Необходим для считывания refresh-токена</param>
    public Task LogoutAsync(IRequestCookieCollection cookies);
}
