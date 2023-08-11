using NotesWebApi.Domains.Entities;
using NotesWebApi.Services.Dto.Note;

namespace NotesWebApi.Services.DtoConverters;

public interface INoteDtoConverter
{
    public Task<Note> ToNote(NoteCreationDto noteDto);
    public NoteInfoDto ToNoteInfoDto(Note note);
}
