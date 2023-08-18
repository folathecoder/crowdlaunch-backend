using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MARKETPLACEAPI.Interfaces;
using AutoMapper;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/nftlikes/[controller]")]
public class NftLikeController : ControllerBase
{
    private readonly INftLikeService _nftLikeService;
    private readonly INftService _nftService;
    private readonly IMapper _mapper;

    public NftLikeController(INftLikeService nftLikeService, INftService nftService, IMapper mapper)
    {
        _nftLikeService = nftLikeService;
        _nftService = nftService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<NftLike>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _nftLikeService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(NftLike), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var nftLike = await _nftLikeService.GetAsync(id);

        if (nftLike is null)
        {
            return NotFound();
        }

        return Ok(nftLike);
    }

    [HttpPost]
    [Authorize]
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
            return Ok(nftLike);
        }

        var newLike = _mapper.Map<NftLike>(newNftLike);
        newLike.userId = userId;
        await _nftLikeService.CreateAsync(newLike);

        nft.noOfLikes += 1;
        nft.updatedAt = DateTime.UtcNow;
        await _nftService.UpdateAsync(nft.nftId!, nft);

        return CreatedAtAction(nameof(Get), new { id = newLike.nftLikeId }, newLike);
    }

    [HttpPatch("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, NftLike updatedNftLike)
    {
        var nftLike = await _nftLikeService.GetAsync(id);

        if (nftLike is null)
        {
            return NotFound();
        }

        var nftLikeUpdate = _mapper.Map(updatedNftLike, nftLike);

        await _nftLikeService.UpdateAsync(id, nftLikeUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = HttpContext.Request.Headers["userId"].ToString();
        var nftLike = await _nftLikeService.GetAsync(id);

        if (nftLike is null)
        {
            return NoContent();
        }

        if (nftLike.userId != userId)
        {
            return NoContent();
        }

        var nft = await _nftService.GetAsync(nftLike.nftId!);

        await _nftLikeService.RemoveAsync(id);

        nft!.noOfLikes -= 1;
        nft.updatedAt = DateTime.UtcNow;
        await _nftService.UpdateAsync(nft.nftId!, nft);

        return NoContent();
    }

    [HttpGet("get-by-nft-id")]
    [ProducesResponseType(typeof(NftLike), 200)]
    public async Task<IActionResult> GetNftLikeByNftId([FromQuery] string nftId)
    {
        var nftLike = await _nftLikeService.GetNftLikeByNftId(nftId);

        if (nftLike is null)
        {
            return NotFound();
        }

        return Ok(nftLike);
    }

    [HttpGet("get-by-user-id")]
    [ProducesResponseType(typeof(IList<NftLike>), 200)]
    public async Task<IActionResult> GetNftLikesByUserId([FromQuery] string userId) =>
        Ok(await _nftLikeService.GetNftLikesByUserId(userId));


    [HttpGet("get-by-user-id-and-nft-id")]
    [ProducesResponseType(typeof(NftLike), 200)]
    public async Task<IActionResult> GetNftLikeByUserIdAndNftId([FromQuery] string userId, [FromQuery] string nftId)
    {
        var nftLike = await _nftLikeService.GetNftLikeByUserIdAndNftId(userId, nftId);

        if (nftLike is null)
        {
            return NotFound();
        }

        return Ok(nftLike);
    }
}