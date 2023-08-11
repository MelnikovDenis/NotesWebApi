using NotesWebApi.Services.Dto.NotesGroup;

namespace NotesWebApi.Services.CrudServices;

public interface INotesGroupCrudService
{
    public Task<List<NotesGroupInfoDto>> GetAll();
    public Task<NotesGroupGetDto> GetByIdAsync(Guid id);
    public Task<NotesGroupInfoDto> CreateAsync(NotesGroupCreationDto notesGroupDto);
    public Task<NotesGroupInfoDto> UpdateAsync(NotesGroupUpdateDto notesGroupUpdateDto);
    public Task<NotesGroupInfoDto> DeleteAsync(Guid id);
}