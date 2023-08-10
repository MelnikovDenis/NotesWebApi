using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userDto)
    {
        return Ok(await AuthService.RegisterAsync(userDto));
    }
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(UserLoginDto userDto)
    {
        return Ok(await AuthService.LoginAsync(userDto));
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken() 
    {
        return Ok(await AuthService.RefreshTokenAsync());
    }
    [HttpPost("AllLogout"), Authorize]
    public async Task<IActionResult> AllLogout() 
    {
        await AuthService.AllLogoutAsync();
        return Ok();
    }
    [HttpPost("Logout"), Authorize]
    public async Task<IActionResult> LogoutAsync() 
    {
        await AuthService.LogoutAsync();
        return Ok();
    }
}