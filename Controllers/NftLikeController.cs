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
[Route("api/nftlikes/[controller]")]
public class NftLikeController : ControllerBase
{
  private readonly NftLikeService _nftLikeService;
  private readonly NftService _nftService;

  public NftLikeController(NftLikeService nftLikeService, NftService nftService)
  {
    _nftLikeService = nftLikeService;
    _nftService = nftService;
  }

  [HttpGet]
  public async Task<List<NftLike>> Get() =>
      await _nftLikeService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<NftLike>> Get(string id)
  {
    var nftLike = await _nftLikeService.GetAsync(id);

    if (nftLike is null)
    {
      return NotFound();
    }

    return nftLike;
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Post(NftLikeCreateDto newNftLike)
  {
    var userId = HttpContext.Request.Headers["userId"].ToString();
    var nft = await _nftService.GetAsync(newNftLike.nftId);

    if (nft is null)
    {
      return NotFound("NFT not found");
    }

    var nftLike = await _nftLikeService.GetNftLikeByUserIdAndNftId(userId, newNftLike.nftId);

    if (nftLike != null)
    {
      return BadRequest("You already liked this NFT");
    }

    var newLike = new NftLike
    {
      nftId = newNftLike.nftId,
      userId = userId,
    };
    await _nftLikeService.CreateAsync(newLike);

    nft.noOfLikes += 1;
    nft.updatedAt = DateTime.UtcNow;
    await _nftService.UpdateAsync(nft.nftId!, nft);

    return CreatedAtAction(nameof(Get), new { id = newLike.nftLikeId }, newLike);
  }

  [HttpPatch("{id:length(24)}")]
  public async Task<IActionResult> Update(string id, NftLike updatedNftLike)
  {
    var nftLike = await _nftLikeService.GetAsync(id);

    if (nftLike is null)
    {
      return NotFound();
    }

    updatedNftLike.nftLikeId = nftLike.nftLikeId;

    await _nftLikeService.UpdateAsync(id, updatedNftLike);

    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> Delete(string id)
  {
    var userId = HttpContext.Request.Headers["userId"].ToString();
    var nftLike = await _nftLikeService.GetAsync(id);

    if (nftLike is null)
    {
      return NotFound();
    }

    if (nftLike.userId != userId)
    {
      return BadRequest("You can only delete your own likes");
    }

    var nft = await _nftService.GetAsync(nftLike.nftId!);

    await _nftLikeService.RemoveAsync(id);

    nft!.noOfLikes -= 1;
    nft.updatedAt = DateTime.UtcNow;
    await _nftService.UpdateAsync(nft.nftId!, nft);

    return NoContent();
  }

  [HttpGet("get-by-nft-id")]
  public async Task<ActionResult<NftLike>> GetNftLikeByNftId([FromQuery] string nftId)
  {
    var nftLike = await _nftLikeService.GetNftLikeByNftId(nftId);

    if (nftLike is null)
    {
      return NotFound();
    }

    return nftLike;
  }

  [HttpGet("get-by-user-id")]
  public async Task<List<NftLike>> GetNftLikesByUserId([FromQuery] string userId) =>
      await _nftLikeService.GetNftLikesByUserId(userId);


  [HttpGet("get-by-user-id-and-nft-id")]
  public async Task<ActionResult<NftLike>> GetNftLikeByUserIdAndNftId([FromQuery] string userId, [FromQuery] string nftId)
  {
    var nftLike = await _nftLikeService.GetNftLikeByUserIdAndNftId(userId, nftId);

    if (nftLike is null)
    {
      return NotFound();
    }

    return nftLike;
  }
}