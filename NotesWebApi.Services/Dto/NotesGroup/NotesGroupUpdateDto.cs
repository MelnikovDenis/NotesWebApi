namespace NotesWebApi.Services.Dto.NotesGroup;

public class NotesGroupUpdateDto
{
    public Guid Id { get; set; }
    public string NewTitle { get; set; } = null!;
}
