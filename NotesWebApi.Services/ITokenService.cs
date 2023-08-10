using Microsoft.AspNetCore.Http;
using NotesWebApi.Domains.Entities;

namespace NotesWebApi.Services;
/// <summary>
///  Интерфейс сервиса выпуска, получения и удаления токенов
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
    public Task SetRefreshTokenAsync(RefreshToken refreshToken);
    /// <summary>
    ///  Удаляет из базы данных истёкшие refresh-токены, связанные с пользователем
    /// </summary>
    public Task DeleteExpiredTokensAsync(User user);
    /// <summary>
    ///  Извлекает refresh-токен из cookie
    /// </summary>
    public Guid GetRefreshToken();
    /// <summary>
    ///  Находит пользователя по claim'ам
    /// </summary>
    public Task<User> GetUserFromClaims();
}
