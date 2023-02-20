using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dontnet7_vue_auth.api.Services.AuthService;

public class RegisterService : IRegisterService
{
    private readonly DataContext _dataContext;

    public RegisterService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private static User _user = new User();


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
    
    
    private void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}

