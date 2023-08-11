namespace NotesWebApi.Services.Dto.Note;

public class NoteUpdateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public Guid GroupId { get; set; }
}
