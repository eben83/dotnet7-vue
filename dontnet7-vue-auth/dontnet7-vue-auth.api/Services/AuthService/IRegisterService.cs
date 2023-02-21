using Microsoft.AspNetCore.Mvc;

namespace dontnet7_vue_auth.api.Services.AuthService;

public interface IRegisterService
{
    Task<ActionResult<List<User>>> Register(UserDto userDto);
    Task<ActionResult<string>> Login(UserDto userDto);
}