using NotesWebApi.Services.Dto.Note;
namespace NotesWebApi.Services.Dto.NotesGroup;

public class NotesGroupGetDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime LastUpdateTime { get; set; }
    public List<NoteInfoDto> Notes { get; set; } = new List<NoteInfoDto>();
}
