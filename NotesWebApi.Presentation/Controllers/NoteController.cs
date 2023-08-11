using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesWebApi.Services.CrudServices;
using NotesWebApi.Services.Dto.Note;

namespace NotesWebApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private INoteCrudService NoteCrudService { get; }

        public NoteController(INoteCrudService noteCrudService)
        {
            NoteCrudService = noteCrudService;
        }

        [HttpPost("Create"), Authorize]
        public async Task<IActionResult> Create(NoteCreationDto noteDto) 
        {
            return Ok(await NoteCrudService.CreateAsync(noteDto));
        }
        [HttpPatch("Update"), Authorize]
        public async Task<IActionResult> Update(NoteUpdateDto noteDto)
        {
            return Ok(await NoteCrudService.UpdateAsync(noteDto));
        }
        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await NoteCrudService.DeleteAsync(id));
        }
        [HttpGet("GetById"), Authorize]
        public async Task<IActionResult> GetById(Guid id) 
        {
            return Ok(await NoteCrudService.GetByIdAsync(id));
        }
    }
}
