using Microsoft.AspNetCore.Http;
using NotesWebApi.Services.Dto.User;
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
    public Task<string> LoginAsync(UserLoginDto userDto);
    /// <summary>
    ///  Получение нового access-токена
    /// </summary>
    /// <param name="cookies">Необходим для считывания refresh-токена</param>
    public Task<string> RefreshTokenAsync();
    /// <summary>
    ///  Удаление всех refresh-токенов пользователя из базы данных
    /// </summary>
    public Task AllLogoutAsync();
    /// <summary>
    ///  Удаление текущего refresh-токена пользователя из базы данных
    /// </summary>
    /// <param name="cookies">Необходим для считывания refresh-токена</param>
    public Task LogoutAsync();
}
