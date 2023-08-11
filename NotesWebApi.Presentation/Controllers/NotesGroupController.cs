using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesWebApi.Infrastructure.CrudServiceslmplementations;
using NotesWebApi.Services.CrudServices;
using NotesWebApi.Services.Dto.NotesGroup;

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
        return Ok(await NotesGroupCrudService.CreateAsync(notesGroupDto));
    }
    [HttpPatch("Update"), Authorize]
    public async Task<IActionResult> Update(NotesGroupUpdateDto notesGroupDto) 
    {
        return Ok(await NotesGroupCrudService.UpdateAsync(notesGroupDto));
    }
    [HttpDelete("Delete"), Authorize]
    public async Task<IActionResult> Delete(Guid id) 
    {
        return Ok(await NotesGroupCrudService.DeleteAsync(id));
    }
    [HttpGet("GetById"), Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await NotesGroupCrudService.GetByIdAsync(id));
    }
    [HttpGet("GetAll"), Authorize]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await NotesGroupCrudService.GetAll());
    }
}
