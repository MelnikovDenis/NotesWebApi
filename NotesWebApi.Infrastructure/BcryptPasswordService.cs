using Microsoft.Extensions.Configuration;
using NotesWebApi.Services;

namespace NotesWebApi.Infrastructure;

public class BcryptPasswordService : IPasswordService
{
    public string CreatePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool VerifyPasswordHash(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
