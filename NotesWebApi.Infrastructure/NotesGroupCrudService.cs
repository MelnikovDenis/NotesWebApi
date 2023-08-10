using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using NotesWebApi.Services.DtoConverters;

namespace NotesWebApi.Infrastructure;

public class NotesGroupCrudService : INotesGroupCrudService
{
    private ApplicationContext Context { get; }
    private INotesGroupDtoConverter NotesGroupDtoConverter { get;  }

    public NotesGroupCrudService(ApplicationContext context, INotesGroupDtoConverter notesGroupDtoConverter)
    {
        Context = context;
        NotesGroupDtoConverter = notesGroupDtoConverter;
    }

    public async Task<NotesGroupInfoDto> Create(NotesGroupCreationDto notesGroupDto)
    {
        var notesGroup = await NotesGroupDtoConverter.ToNotesGroup(notesGroupDto);
        Context.Add(notesGroup);
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }
}
