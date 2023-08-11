using NotesWebApi.Domains.Entities;
using NotesWebApi.Domains.Persistence;
using NotesWebApi.Services.Dto.Note;
using NotesWebApi.Services.DtoConverters;
using System.Net;

namespace NotesWebApi.Infrastructure.DtoConverters;

public class NoteDtoConverter : INoteDtoConverter
{
    public ApplicationContext Context { get; }

    public NoteDtoConverter(ApplicationContext context)
    {
        Context = context;
    }

    public async Task<Note> ToNote(NoteCreationDto noteDto)
    {
        return new Note 
        {
            Id = Guid.NewGuid(),
            Title = noteDto.Title,
            Text = noteDto.Text,
            CreationTime = DateTime.UtcNow,
            LastUpdateTime = DateTime.UtcNow,
            Group = await Context.NotesGroups.FindAsync(noteDto.GroupId)
                ?? throw new HttpRequestException("Incorrect GroupId.", null, HttpStatusCode.BadRequest)
        };
    }

    public NoteInfoDto ToNoteInfoDto(Note note)
    {
        return new NoteInfoDto
        {
            Id = note.Id,
            Title = note.Title,
            Text = note.Text,
            CreationTime = note.CreationTime,
            LastUpdateTime = note.LastUpdateTime,
            GroupId = note.Group.Id   
        };
    }
}
