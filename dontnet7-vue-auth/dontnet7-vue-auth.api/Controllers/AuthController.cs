
using dontnet7_vue_auth.api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dontnet7_vue_auth.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IRegisterService _registerService;
    

    public AuthController(IRegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Register(UserDto userDto)
    {
        var data = await _registerService.Register(userDto);

        if (data is null)
        {
            return BadRequest("User Name already exists");
        }

        return Ok(data);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(UserDto userDto)
    {
        var data = await _registerService.Login(userDto);

        if (data == null || String.IsNullOrEmpty(data.Value))
        {
            return Unauthorized("Login Failed");
        }
        return Ok(data);
    }

}