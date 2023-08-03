using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MARKETPLACEAPI.Controllers;


[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/auth/[controller]")]
public class AuthController : ControllerBase
{
  private readonly UserService _userService;
  private readonly AuthService _authService;

  public AuthController(UserService userService, AuthService authService)
  {
    _userService = userService;
    _authService = authService;
  }


  [HttpPost("register")]
  [ProducesResponseType(typeof(SignInResponseDto), 201)]
  public async Task<IActionResult> Register(SignInRegisterDto regDto)
  {
    var user = await _userService.GetUserByWalletAddress(regDto.walletAddress);
    if (user != null)
    {
      var res = new SignInResponseDto
      {
        walletAddress = user.walletAddress,
        accountCreated = false,
        accountSignedIn = true,
        accountExists = true,
        invalidAddress = false,
        errorMessage = null,
        token = _authService.GenerateToken(user),
      };
      return Ok(res);
    }

    var newUser = new User
    {
      userName = regDto.userName,
      walletAddress = regDto.walletAddress,
      socials = regDto.socials,
    };
    await _userService.CreateAsync(newUser);

    var response = new SignInResponseDto
    {
      walletAddress = newUser.walletAddress,
      accountCreated = true,
      accountSignedIn = true,
      accountExists = false,
      invalidAddress = false,
      errorMessage = null,
      token = _authService.GenerateToken(newUser),
    };

    return Ok(response);
  }


  [HttpPost("login")]
  [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status200OK)]
  public async Task<IActionResult> Login(LoginDto loginDto)
  {
    var user = await _userService.GetUserByWalletAddress(loginDto.walletAddress!);

    if (user is null)
    {
      return Unauthorized(new SignInResponseDto
      {
        walletAddress = null,
        accountCreated = false,
        accountSignedIn = false,
        accountExists = false,
        invalidAddress = true,
        errorMessage = "Invalid wallet address",
      });
    }

    var token = _authService.GenerateToken(user);

    return Ok(new SignInResponseDto
    {
      walletAddress = user.walletAddress,
      accountCreated = false,
      accountSignedIn = true,
      accountExists = true,
      invalidAddress = false,
      errorMessage = null,
      token = token,
    });
  }
}

