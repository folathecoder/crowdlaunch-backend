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
[Route("api/user-nfts/[controller]")]
public class UserNftController : ControllerBase
{
    private readonly IUserNftService _userNftService;
    private readonly IMapper _mapper;

    public UserNftController(IUserNftService userNftService, IMapper mapper)
    {
        _userNftService = userNftService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<UserNft>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _userNftService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(UserNft), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var userNft = await _userNftService.GetAsync(id);

        if (userNft is null)
        {
            return NotFound();
        }

        return Ok(userNft);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(UserNftCreateDto newUserNft)
    {
        var userId = HttpContext.Request.Headers["userId"].ToString();
        var existingUserNft = await _userNftService.GetUserNftByUserIdAndNftId(userId, newUserNft.nftId!);

        if (existingUserNft != null)
        {
            return Ok(existingUserNft);
        }

        var userNft = _mapper.Map<UserNft>(newUserNft);
        userNft.userId = userId;
        await _userNftService.CreateAsync(userNft);

        return CreatedAtAction(nameof(Get), new { id = userNft.userNftId }, userNft);
    }

    [HttpPatch("{id:length(24)}")]
    [Authorize]
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
    [Authorize]
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
    [ProducesResponseType(typeof(IList<UserNft>), 200)]
    public async Task<IActionResult> GetByUserId(string userId) =>
        Ok(await _userNftService.GetUserNftByUserId(userId));


    [HttpGet("get-by-nftid")]
    [ProducesResponseType(typeof(UserNft), 200)]
    public async Task<IActionResult> GetByNftId([FromQuery] string nftId)
    {
        var userNft = await _userNftService.GetUserNftByNftId(nftId);
        if (userNft is null)
        {
            return NotFound();
        }
        return Ok(userNft);
    }

    [HttpGet("get-by-userid-nftid")]
    [ProducesResponseType(typeof(UserNft), 200)]
    public async Task<IActionResult> GetByUserIdNftId([FromQuery] string userId, [FromQuery] string nftId)
    {
        var userNft = await _userNftService.GetUserNftByUserIdAndNftId(userId, nftId);
        if (userNft is null)
        {
            return NotFound();
        }
        return Ok(userNft);
    }

}