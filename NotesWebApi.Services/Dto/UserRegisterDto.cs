namespace NotesWebApi.Services.Dto;

public class UserRegisterDto
{
    public string Email { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Password { get; set; } = null!;
}
