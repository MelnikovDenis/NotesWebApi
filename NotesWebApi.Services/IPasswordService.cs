namespace NotesWebApi.Services;

public interface IPasswordService
{
    public string CreatePasswordHash(string password);
    public bool VerifyPasswordHash(string hash, string password);
}
