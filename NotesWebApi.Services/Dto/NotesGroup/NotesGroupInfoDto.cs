namespace NotesWebApi.Services.Dto.NotesGroup;

public class NotesGroupInfoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime LastUpdateTime { get; set; }
}
