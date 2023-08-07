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
[Route("api/user-nfts/[controller]")]
public class UserNftController : ControllerBase
{
    private readonly UserNftService _userNftService;

    public UserNftController(UserNftService userNftService) =>
        _userNftService = userNftService;

    [HttpGet]
    public async Task<List<UserNft>> Get() =>
        await _userNftService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserNft>> Get(string id)
    {
        var userNft = await _userNftService.GetAsync(id);

        if (userNft is null)
        {
            return NotFound();
        }

        return userNft;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserNftCreateDto newUserNft)
    {   
        var userId = HttpContext.Request.Headers["userId"].ToString();
        var existingUserNft = await _userNftService.GetUserNftByUserIdAndNftId(userId, newUserNft.nftId!);

        if (existingUserNft != null)
        {
            return Conflict("User already owns this NFT");
        }

        var userNft = new UserNft {
            userId = userId,
            nftId = newUserNft.nftId!
        };
        await _userNftService.CreateAsync(userNft);

        return CreatedAtAction(nameof(Get), new { id = userNft.userNftId }, userNft);
    }

    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UserNft updatedUserNft)
    {
        var userNft = await _userNftService.GetAsync(id);

        if (userNft is null)
        {
            return NotFound();
        }

        updatedUserNft.userNftId = userNft.userNftId;

        await _userNftService.UpdateAsync(id, updatedUserNft);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userNft = await _userNftService.GetAsync(id);

        if (userNft is null)
        {
            return NotFound();
        }

        await _userNftService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("get-by-userid")]
    public async Task<List<UserNft>> GetByUserId(string userId) =>
        await _userNftService.GetUserNftByUserId(userId);


    [HttpGet("get-by-nftid")]
    public async Task<ActionResult<UserNft?>> GetByNftId([FromQuery] string nftId)  {
        var userNft = await _userNftService.GetUserNftByNftId(nftId);
        if (userNft is null)
        {
            return NotFound();
        }
        return userNft;
    }

    [HttpGet("get-by-userid-nftid")]
    public async Task<ActionResult<UserNft?>> GetByUserIdNftId([FromQuery] string userId, [FromQuery] string nftId)  {
        var userNft = await _userNftService.GetUserNftByUserIdAndNftId(userId, nftId);
        if (userNft is null)
        {
            return NotFound();
        }
        return userNft;
    }
       
}