using Microsoft.EntityFrameworkCore;
using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services.CrudServices;
using NotesWebApi.Services.Dto.Note;
using NotesWebApi.Services.DtoConverters;
using System.Net;

namespace NotesWebApi.Infrastructure.CrudServiceslmplementations;

public class NoteCrudService : INoteCrudService
{
    private INoteDtoConverter NoteDtoConverter { get; }
    private ApplicationContext Context { get; }

    public NoteCrudService(INoteDtoConverter noteDtoConverter, ApplicationContext context)
    {
        NoteDtoConverter = noteDtoConverter;
        Context = context;
    }

    public async Task<NoteInfoDto> CreateAsync(NoteCreationDto noteDto)
    {
        var note = await NoteDtoConverter.ToNote(noteDto);
        Context.Notes.Add(note);
        SetGroupUpdateTime(note);
        await Context.SaveChangesAsync();
        return NoteDtoConverter.ToNoteInfoDto(note);
    }

    public async Task<NoteInfoDto> DeleteAsync(Guid id)
    {
        var note = await Context.Notes.Include(n => n.Group).FirstOrDefaultAsync(n => n.Id == id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        SetGroupUpdateTime(note);
        Context.Notes.Remove(note);
        await Context.SaveChangesAsync();
        return NoteDtoConverter.ToNoteInfoDto(note);
    }

    public async Task<NoteInfoDto> UpdateAsync(NoteUpdateDto noteDto)
    {
        var note = await Context.Notes.Include(n => n.Group).FirstOrDefaultAsync(n => n.Id == noteDto.Id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        var notesGroup = await Context.NotesGroups.FindAsync(noteDto.GroupId)
             ?? throw new HttpRequestException("Incorrect GroupId.", null, HttpStatusCode.BadRequest);
        SetGroupUpdateTime(note);
        note.Title = noteDto.Title;
        note.Text = noteDto.Text;
        note.LastUpdateTime = DateTime.UtcNow;
        note.Group = notesGroup;
        Context.Notes.Update(note);
        await Context.SaveChangesAsync();
        return NoteDtoConverter.ToNoteInfoDto(note);
    }

    public async Task<NoteInfoDto> GetByIdAsync(Guid id)
    {
        var note = await Context.Notes.FindAsync(id)
            ?? throw new HttpRequestException("Incorrect Id.", null, HttpStatusCode.BadRequest);
        return NoteDtoConverter.ToNoteInfoDto(note);
    }
    private void SetGroupUpdateTime(Note note) 
    {
        note.Group.LastUpdateTime = DateTime.UtcNow;
        Context.NotesGroups.Update(note.Group);
    }
}
