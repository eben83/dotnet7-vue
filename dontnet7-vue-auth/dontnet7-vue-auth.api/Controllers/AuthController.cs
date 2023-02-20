
using dontnet7_vue_auth.api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<User>> Register(UserDto userDto)
    {
        var data = await _registerService.Register(userDto);

        return Ok(data);
    }

}