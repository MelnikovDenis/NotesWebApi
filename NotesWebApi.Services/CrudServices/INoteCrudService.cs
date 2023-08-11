using NotesWebApi.Services.Dto.Note;

namespace NotesWebApi.Services.CrudServices;

public interface INoteCrudService
{
    public Task<NoteInfoDto> GetByIdAsync(Guid id);
    public Task<NoteInfoDto> CreateAsync(NoteCreationDto noteDto);
    public Task<NoteInfoDto> DeleteAsync(Guid id);
    public Task<NoteInfoDto> UpdateAsync(NoteUpdateDto noteDto);
}
