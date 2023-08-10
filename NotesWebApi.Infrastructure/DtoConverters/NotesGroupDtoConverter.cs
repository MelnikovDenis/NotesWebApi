using NotesWebApi.Domains.Entities;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using NotesWebApi.Services.DtoConverters;

namespace NotesWebApi.Infrastructure.DtoConverters;

public class NotesGroupDtoConverter : INotesGroupDtoConverter
{
    private ITokenService TokenService { get;  }

    public NotesGroupDtoConverter(ITokenService tokenService)
    {
        TokenService = tokenService;
    }

    public async Task<NotesGroup> ToNotesGroup(NotesGroupCreationDto notesGroupDto)
    {
        return new NotesGroup
        {
            Id = Guid.NewGuid(),
            Title = notesGroupDto.Title,
            Author = (await TokenService.GetUserFromClaims()),
            LastUpdateTime = DateTime.UtcNow
        };
    }

    public NotesGroupInfoDto ToNotesGroupInfoDto(NotesGroup notesGroup)
    {
        return new NotesGroupInfoDto
        {
            Id = notesGroup.Id,
            Title = notesGroup.Title,
            LastUpdateTime = notesGroup.LastUpdateTime
        };
    }
}
