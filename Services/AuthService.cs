using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Helpers;

namespace MARKETPLACEAPI.Services;

public class AuthService : IAuthService {
    private readonly IConfiguration _config;
    private readonly IUserService _userService;

    public AuthService(IConfiguration config, IUserService userService) {
        _config = config;
        _userService = userService;
        
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

        TokenHelper _tokenHelper = new TokenHelper(_config);

        var token = _tokenHelper.GenerateToken(user);

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

}
