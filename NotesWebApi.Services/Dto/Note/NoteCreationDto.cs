namespace NotesWebApi.Services.Dto.Note;

public class NoteCreationDto
{
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public Guid GroupId { get; set; }
}
