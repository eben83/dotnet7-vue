using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dontnet7_vue_auth.api.Services.AuthService;

public class RegisterService : IRegisterService
{
    private readonly DataContext _dataContext;
    private static IConfiguration _configuration;
    private static User _user = new User();

    public RegisterService(DataContext dataContext, IConfiguration configuration)
    {
        _configuration = configuration;
        _dataContext = dataContext;
    }

    public async Task<ActionResult<List<User>>> Register(UserDto userDto)
    {
        CreatePassWordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        _user.UserName = userDto.UserName;
        _user.PasswordHash = passwordHash;
        _user.PasswordSalt = passwordSalt;

        _dataContext.Users.Add(_user);
        await _dataContext.SaveChangesAsync();
        return await _dataContext.Users.ToListAsync();
    }

    public Task<ActionResult<string>> Login(UserDto userDto)
    {
        if (_user.UserName != userDto.UserName)
        {
            return Task.FromResult<ActionResult<string>>(string.Empty);
        }
        
        if (VerifyPasswordHash(userDto.Password, _user.PasswordHash, _user.PasswordSalt))
        {
            var token = CreateToken(_user);
            return Task.FromResult<ActionResult<string>>(token);
        }

        return Task.FromResult<ActionResult<string>>(string.Empty);
    }
    
    private static void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        if (password.IsNullOrEmpty())
        {
            return false;
        }

        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

    }

    private static string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName)
        };
        
        var keySting = _configuration.GetSection("AppSettings:Token").Value;

        // var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        // var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my top secret key")
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keySting)
            // _configuration.GetSection("AppSettings:Token").Value)
        );
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}

