using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
[Route("api/user/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PortfolioService _portfolioService;
    private readonly ProjectService _projectService;
    private readonly UserNftService _userNftService;
    private readonly ProjectLikeService _projectLikeService;
    private readonly NftLikeService _nftLikeService;

    public UserController(UserService userService, PortfolioService portfolioService, 
    ProjectService projectService, UserNftService userNftService, 
    ProjectLikeService projectLikeService, NftLikeService nftLikeService) {
        _userService = userService;
        _portfolioService = portfolioService;
        _projectService = projectService;
        _userNftService = userNftService;
        _projectLikeService = projectLikeService;
        _nftLikeService = nftLikeService;
    }
       

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _userService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserDto>> Get(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var portfolios = await _portfolioService.GetPortfolioByUserId(user.userId!);
        var nftWatchlist = await _nftLikeService.GetNftLikesByUserId(user.userId!);
        var projectWatchlist = await _projectLikeService.GetProjectLikesByUserId(user.userId!);
        var ownedNfts = await _userNftService.GetUserNftByUserId(user.userId!);
        var listedProjects = await _projectService.GetProjectsByUserId(user.userId!);

        var userDto = new UserDto {
            user = user,
            portfolios = portfolios,
            nftWatchlist = nftWatchlist,
            projectWatchlist = projectWatchlist,
            ownedNfts = ownedNfts,
            listedProjects = listedProjects
        };

        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.userId }, newUser);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UserUpdateDto updatedUser)
    {
        var id = HttpContext.Request.Headers["userId"].ToString();
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        user.userName = updatedUser.userName;
        user.socials = updatedUser.socials;
        user.updatedAt = updatedUser.updatedAt;


        await _userService.UpdateAsync(id, user);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("get-by-wallet-address")]
    public async Task<ActionResult<UserDto>> GetByWalletAddress([FromHeader] string walletAddress)
    {
        var user = await _userService.GetUserByWalletAddress(walletAddress);

        if (user is null)
        {
            return NotFound();
        }

        var portfolios = await _portfolioService.GetPortfolioByUserId(user.userId!);
        var nftWatchlist = await _nftLikeService.GetNftLikesByUserId(user.userId!);
        var projectWatchlist = await _projectLikeService.GetProjectLikesByUserId(user.userId!);
        var ownedNfts = await _userNftService.GetUserNftByUserId(user.userId!);
        var listedProjects = await _projectService.GetProjectsByUserId(user.userId!);

        var userDto = new UserDto {
            user = user,
            portfolios = portfolios,
            nftWatchlist = nftWatchlist,
            projectWatchlist = projectWatchlist,
            ownedNfts = ownedNfts,
            listedProjects = listedProjects
        };

        return Ok(userDto);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {   
        var userId = HttpContext.Request.Headers["userId"].ToString();
        var user = await _userService.GetAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        var portfolios = await _portfolioService.GetPortfolioByUserId(user.userId!);
        var nftWatchlist = await _nftLikeService.GetNftLikesByUserId(user.userId!);
        var projectWatchlist = await _projectLikeService.GetProjectLikesByUserId(user.userId!);
        var ownedNfts = await _userNftService.GetUserNftByUserId(user.userId!);
        var listedProjects = await _projectService.GetProjectsByUserId(user.userId!);

        var userDto = new UserDto {
            user = user,
            portfolios = portfolios,
            nftWatchlist = nftWatchlist,
            projectWatchlist = projectWatchlist,
            ownedNfts = ownedNfts,
            listedProjects = listedProjects
        };

        return Ok(userDto);
    }

}