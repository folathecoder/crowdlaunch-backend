using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.dto;

namespace MARKETPLACEAPI.Services;

public class AuthService : IAuthService {
    private readonly IConfiguration _config;
    private readonly UserService _userService;

    public AuthService(IConfiguration config, UserService userService) {
        _config = config;
        _userService = userService;
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

    public async Task<SignInResponseDto> Authenticate(LoginDto loginDto) 
    {
        var user = await _userService.GetAsync(loginDto.walletAddress!);

        if (user is null)
        {
            return new SignInResponseDto
            {
                walletAddress = null,
                accountCreated = false,
                accountSignedIn = false,
                accountExists = false,
                invalidAddress = true,
                errorMessage = "Invalid wallet address",
            };
        }

        var token = GenerateToken(user);

        return new SignInResponseDto
        {
            walletAddress = user.walletAddress,
            accountCreated = false,
            accountSignedIn = true,
            accountExists = true,
            invalidAddress = false,
            errorMessage = null,
            token = token,
        };
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
