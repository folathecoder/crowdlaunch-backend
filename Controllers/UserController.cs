using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Interfaces;
using MARKETPLACEAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/user/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPortfolioService _portfolioService;
    private readonly IProjectService _projectService;
    private readonly IUserNftService _userNftService;
    private readonly IProjectLikeService _projectLikeService;
    private readonly INftLikeService _nftLikeService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IPortfolioService portfolioService,
    IProjectService projectService, IUserNftService userNftService,
    IProjectLikeService projectLikeService, INftLikeService nftLikeService, IMapper mapper)
    {
        _userService = userService;
        _portfolioService = portfolioService;
        _projectService = projectService;
        _userNftService = userNftService;
        _projectLikeService = projectLikeService;
        _nftLikeService = nftLikeService;
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(typeof(IList<User>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _userService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> Get(string id)
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

        var userDto = new UserDto
        {
            user = user,
            portfolios = portfolios,
            nftWatchlist = nftWatchlist,
            projectWatchlist = projectWatchlist,
            ownedNfts = ownedNfts,
            listedProjects = listedProjects
        };

        return Ok(userDto);
    }


    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UserUpdateDto updatedUser)
    {
        var id = HttpContext.Request.Headers["userId"].ToString();
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        var updatedUserModel = _mapper.Map(updatedUser, user);


        await _userService.UpdateAsync(id, updatedUserModel);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = HttpContext.Request.Headers["userId"].ToString();
        if (userId != id)
        {
            return Unauthorized();
        }
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("get-by-wallet-address")]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetByWalletAddress([FromHeader] string walletAddress)
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

        var userDto = new UserDto
        {
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
    [Authorize]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> GetMe()
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

        var userDto = new UserDto
        {
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