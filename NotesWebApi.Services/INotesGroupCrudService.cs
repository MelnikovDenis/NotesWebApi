using NotesWebApi.Services.Dto;

namespace NotesWebApi.Services;

public interface INotesGroupCrudService
{
    public Task<NotesGroupInfoDto> Create(NotesGroupCreationDto notesGroupDto); 
    public Task<NotesGroupInfoDto> Update(NotesGroupUpdateDto notesGroupUpdateDto);
    public Task<NotesGroupInfoDto> Delete(Guid id);
}
