using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesWebApi.Services;
using NotesWebApi.Services.Dto;

namespace NotesWebApi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private IAuthService AuthService { get; }

    public AuthenticationController(IAuthService authService)
    {
        AuthService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegisterDto userDto)
    {
        return Ok(await AuthService.RegisterAsync(userDto));
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        return Ok(await AuthService.LoginAsync(userDto, Response));
    }
}