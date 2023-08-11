namespace NotesWebApi.Services.Dto;

public class NotesGroupUpdateDto
{
    public Guid Id { get; set; }
    public string NewTitle { get; set; } = null!;
}
