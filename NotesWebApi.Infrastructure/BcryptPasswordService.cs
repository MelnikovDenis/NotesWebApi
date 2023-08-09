using NotesWebApi.Services;

namespace NotesWebApi.Infrastructure;

public class BcryptPasswordService : IPasswordService
{
    public string CreatePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, BCrypt.Net.HashType.SHA512);
    }
    public bool VerifyPasswordHash(string hash, string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash, BCrypt.Net.HashType.SHA512);
    }
}
