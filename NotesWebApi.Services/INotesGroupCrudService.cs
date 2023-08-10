using NotesWebApi.Services.Dto;

namespace NotesWebApi.Services;

public interface INotesGroupCrudService
{
    public Task<NotesGroupInfoDto> Create(NotesGroupCreationDto notesGroupDto);
}
