namespace NotesWebApi.Services.Dto.Note;

public class NoteInfoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime CreationTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public Guid GroupId { get; set; }
}
