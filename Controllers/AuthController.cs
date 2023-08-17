using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


namespace MARKETPLACEAPI.Controllers;


[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/auth/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IUserService _userService;
  private readonly IAuthService _authService;
  
  private readonly IConfiguration _config;
  private readonly IMapper _mapper;

  public AuthController(IUserService userService, IAuthService authService, IConfiguration config, IMapper mapper)
  {
    _userService = userService;
    _authService = authService;
    _config = config;
    _mapper = mapper;

  }
  

  [HttpPost("register")]
  [ProducesResponseType(typeof(SignInResponseDto), 201)]
  public async Task<IActionResult> Register(SignInRegisterDto regDto)
  {
    var user = await _userService.GetUserByWalletAddress(regDto.walletAddress);
    TokenHelper _tokenHelper = new TokenHelper(_config);
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
        token = _tokenHelper.GenerateToken(user),
      };
      return Ok(res);
    } 
    
    var newUser = _mapper.Map<User>(regDto);
    await _userService.CreateAsync(newUser);

    var response = new SignInResponseDto
    {
      walletAddress = newUser.walletAddress,
      accountCreated = true,
      accountSignedIn = true,
      accountExists = false,
      invalidAddress = false,
      errorMessage = null,
      token = _tokenHelper.GenerateToken(newUser),
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
    TokenHelper _tokenHelper = new TokenHelper(_config);
    var token = _tokenHelper.GenerateToken(user);

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

