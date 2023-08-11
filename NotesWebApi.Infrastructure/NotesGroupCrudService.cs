using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;
using NotesWebApi.Services.DtoConverters;
using System.Net;

namespace NotesWebApi.Infrastructure;

public class NotesGroupCrudService : INotesGroupCrudService
{
    private ApplicationContext Context { get; }
    private INotesGroupDtoConverter NotesGroupDtoConverter { get; }
    private ITokenService TokenService { get; }
    public NotesGroupCrudService(ApplicationContext context, INotesGroupDtoConverter notesGroupDtoConverter, ITokenService tokenService)
    {
        Context = context;
        NotesGroupDtoConverter = notesGroupDtoConverter;
        TokenService = tokenService;
    }
    public async Task<NotesGroupInfoDto> Create(NotesGroupCreationDto notesGroupDto)
    {
        var notesGroup = await NotesGroupDtoConverter.ToNotesGroup(notesGroupDto);
        if(await IsDuplicate(notesGroup, await TokenService.GetUserFromClaims()))
            throw new HttpRequestException("A group with the same name already exists.", null, HttpStatusCode.BadRequest);
        Context.Add(notesGroup);
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }
    public async Task<NotesGroupInfoDto> Update(NotesGroupUpdateDto notesGroupDto) 
    {
        var notesGroup = await Context.NotesGroups.FirstOrDefaultAsync(g => g.Id == notesGroupDto.Id) 
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
    public async Task<NotesGroupInfoDto> Delete(Guid id)
    {
        var notesGroup = await Context.NotesGroups.FirstOrDefaultAsync(g => g.Id == id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        Context.NotesGroups.Remove(notesGroup);
        await Context.SaveChangesAsync();
        return NotesGroupDtoConverter.ToNotesGroupInfoDto(notesGroup);
    }
}
