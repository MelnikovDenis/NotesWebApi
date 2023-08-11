using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.CrudServices;
using NotesWebApi.Services.Dto.Note;
using NotesWebApi.Services.Dto.NotesGroup;
using NotesWebApi.Services.DtoConverters;
using System.Net;

namespace NotesWebApi.Infrastructure.CrudServiceslmplementations;

public class NotesGroupCrudService : INotesGroupCrudService
{
    private ApplicationContext Context { get; }
    private INoteDtoConverter NoteDtoConverter { get; }
    private INotesGroupDtoConverter NotesGroupDtoConverter { get; }
    private ITokenService TokenService { get; }
    public NotesGroupCrudService(ApplicationContext context, INotesGroupDtoConverter notesGroupDtoConverter, ITokenService tokenService, INoteDtoConverter noteDtoConverter)
    {
        Context = context;
        NotesGroupDtoConverter = notesGroupDtoConverter;
        TokenService = tokenService;
        NoteDtoConverter = noteDtoConverter;
    }
    public async Task<NotesGroupInfoDto> CreateAsync(NotesGroupCreationDto notesGroupDto)
    {
        var notesGroup = await NotesGroupDtoConverter.ToNotesGroup(notesGroupDto);
        if (await IsDuplicate(notesGroup, await TokenService.GetUserFromClaims()))
            throw new HttpRequestException("A group with the same name already exists.", null, HttpStatusCode.BadRequest);
        Context.Add(notesGroup);
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }
    public async Task<NotesGroupInfoDto> UpdateAsync(NotesGroupUpdateDto notesGroupDto)
    {
        var notesGroup = await Context.NotesGroups.FindAsync(notesGroupDto.Id)
           ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        if (await IsDuplicate(notesGroup, await TokenService.GetUserFromClaims()))
            throw new HttpRequestException("A group with the same name already exists.", null, HttpStatusCode.BadRequest);
        notesGroup.Title = notesGroupDto.NewTitle;
        notesGroup.LastUpdateTime = DateTime.UtcNow;
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }
    private async Task<bool> IsDuplicate(NotesGroup notesGroup, User user)
    {
        return await Context.NotesGroups.FirstOrDefaultAsync(g => g.Title == notesGroup.Title && g.Author.Id == user.Id) != null;
    }
    public async Task<NotesGroupInfoDto> DeleteAsync(Guid id)
    {
        var notesGroup = await Context.NotesGroups.FindAsync(id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        Context.NotesGroups.Remove(notesGroup);
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }

    public async Task<NotesGroupGetDto> GetByIdAsync(Guid id)
    {
        var notesGroup = await Context.NotesGroups.Include(g => g.Notes).FirstOrDefaultAsync(g => g.Id == id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        var notesGroupGetDto = new NotesGroupGetDto
        {
            Id = notesGroup.Id,
            Title = notesGroup.Title,
            LastUpdateTime = DateTime.UtcNow,
            Notes = notesGroup.Notes.ConvertAll(NoteDtoConverter.ToNoteInfoDto)
        };
        return notesGroupGetDto;
    }
    public async Task<List<NotesGroupInfoDto>> GetAll()
    {
        var id = (await TokenService.GetUserFromClaims()).Id;
        var groups = await Context.NotesGroups.Where(g => g.Author.Id == id).ToListAsync();
        var groupsDto = groups.ConvertAll(NotesGroupDtoConverter.ToNotesGroupInfoDto);
        return groupsDto;
    }
}
