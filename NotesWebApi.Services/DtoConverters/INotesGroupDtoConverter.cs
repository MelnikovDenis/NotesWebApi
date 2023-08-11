using NotesWebApi.Domains.Entities;
using NotesWebApi.Services.Dto.NotesGroup;

namespace NotesWebApi.Services.DtoConverters;

public interface INotesGroupDtoConverter
{
    public Task<NotesGroup> ToNotesGroup(NotesGroupCreationDto notesGroupDto);
    public NotesGroupInfoDto ToNotesGroupInfoDto(NotesGroup notesGroup);
}
