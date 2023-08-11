using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesGroupController : ControllerBase
{
    private INotesGroupCrudService NotesGroupCrudService { get; }

    public NotesGroupController(INotesGroupCrudService notesGroupCrudService)
    {
        NotesGroupCrudService = notesGroupCrudService;
    }

    [HttpPost("Create"), Authorize]
    public async Task<IActionResult> Create(NotesGroupCreationDto notesGroupDto) 
    {
        return Ok(await NotesGroupCrudService.Create(notesGroupDto));
    }
    [HttpPatch("Update"), Authorize]
    public async Task<IActionResult> Update(NotesGroupUpdateDto notesGroupDto) 
    {
        return Ok(await NotesGroupCrudService.Update(notesGroupDto));
    }
    [HttpDelete("Delete"), Authorize]
    public async Task<IActionResult> Delete(Guid id) 
    {
        return Ok(await NotesGroupCrudService.Delete(id));
    }
}
