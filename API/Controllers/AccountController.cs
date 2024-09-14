using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//! Note that the constructor paramters are syntactic sugar for
//! private field named P or whatever, holding the value of the paramter
//! so if you need to use its value outside 
//! you would need to make a property
public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] //api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
            return BadRequest("Username is taken");
        return Ok();
        /* using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            Username = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new UserDto { 
        Token = tokenService.CreateToken(user),
        Username = user.Username
        }; */
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(
            user => user.Username == loginDto.Username.ToLower()
            );

        if (user == null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid password!");

        return new UserDto
        {
            Token = tokenService.CreateToken(user),
            Username = user.Username
        };
    }
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(user => user.Username.ToLower() == username.ToLower());
    }
}
