using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.Helpers;
public class TokenHelper 
{
    private readonly IConfiguration _config;

    public TokenHelper(IConfiguration config)
    {
        _config = config;
    }
    public string GenerateToken(User user)
    {   
        string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT_KEY not found");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.userId!),
                new Claim(ClaimTypes.Name, user.userName!),           
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var decodedToken = jsonToken as JwtSecurityToken;

        var userId = decodedToken!.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        return userId;
    }

}