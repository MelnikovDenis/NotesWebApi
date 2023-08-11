namespace NotesWebApi.Services.Dto.User;

public class UserRegisterDto
{
    public string Email { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Password { get; set; } = null!;
}
