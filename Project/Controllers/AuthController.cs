using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.UserServices;

namespace Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public IActionResult TestGet()
    {
        return Ok("works");
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterUserDTO req)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var result = await _userService.AddUser(req);

        if (result.Success)
            return Ok();

        return BadRequest(result.Message);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDTO req)
    {
        var result = await _userService.LoginUser(req);

        if (result == "Wrong password") return BadRequest();

        return Ok();
    }
}