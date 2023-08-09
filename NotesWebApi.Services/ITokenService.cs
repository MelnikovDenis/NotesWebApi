using Microsoft.AspNetCore.Http;
using NotesWebApi.Domains.Entities;

namespace NotesWebApi.Services;
/// <summary>
///  Интерфейс сервиса выпуска, подтверждения и удаления токенов
/// </summary>
public interface ITokenService
{
    /// <summary>
    ///  Создание access-токена
    /// </summary>
    public string CreateAccessToken(User user);
    /// <summary>
    ///  Создание refresh-токена
    /// </summary>
    public RefreshToken CreateRefreshToken(User user);
    /// <summary>
    ///  Устанавливает refresh-токен в cookie
    /// </summary>
    /// <param name="cookies">Необходим для установки refresh-токена в cookie</param>
    public Task SetRefreshTokenAsync(RefreshToken refreshToken, IResponseCookies cookies);
    /// <summary>
    ///  Удаляет из базы данных истёкшие cookie, связанные с пользователем
    /// </summary>
    public Task DeleteExpiredTokensAsync(User user);
}
