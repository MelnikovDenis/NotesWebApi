using Microsoft.AspNetCore.Http;
using NotesWebApi.Domains.Entities;

namespace NotesWebApi.Services;

public interface ITokenService
{
    public string CreateAccessToken(User user);
    public RefreshToken CreateRefreshToken(User user);
    public Task SetRefreshTokenAsync(RefreshToken refreshToken, HttpResponse httpResponse);
    public Task DeleteExpiredTokensAsync(User user);
}
