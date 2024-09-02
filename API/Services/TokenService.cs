using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    //! We defined IConfiguration using dependency injection because we need the key
    //! from the config files to sign the JWTs
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? 
            throw new Exception("Cannot access TokenKey from appsettings");
        if (tokenKey.Length < 64)
            throw new Exception("TokenKey must be longer!");
        //! Symmetric keys where we use one key for encryption and decryption
        //! Asymmetric keys have two keys one for encryption and one for decryption
        //! Like ssl certificates, we have public key and secret key

        //! 1) Getting the key as byte[]
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenKey)
        );

        //! 2) Storing the claims, right now we only want to store the user's username so we use NameIdentifier
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.Username),
        };

        //! 3) Creating the descriptor to store the claims in the token subject, setting expiry date and setting the signing credentials using HMASCHA512
        //! Also the signing credentials contains the key stored in the config files just like in Node.js
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature)
        };

        //! 4) Creating the token handler, creating the token and writing the token as response using the token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
