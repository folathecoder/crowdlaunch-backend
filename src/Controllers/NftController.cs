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
public class NftController : ControllerBase
{
  private readonly NftService _nftService;
  private readonly CategoryService _categoryService;

  public NftController(NftService nftService, CategoryService categoryService)
  {
    _nftService = nftService;
    _categoryService = categoryService;
  }

  [HttpGet]
  public async Task<List<Nft>> Get() =>
      await _nftService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<NftDto>> Get(string id)
  {
    var nft = await _nftService.GetAsync(id);
    if (nft is null)
    {
      return NotFound();
    }

    var nftDto = new NftDto
    {
      nft = nft,
      category = await _categoryService.GetAsync(nft.categoryId),
    };

    return nftDto;
  }

  [HttpPost]
  public async Task<IActionResult> Post(NftCreateDto newNft)
  {
    var userId = HttpContext.Request.Headers["userId"].ToString();

    var nft = new Nft
    {
      nftName = newNft.nftName,
      nftDescription = newNft.nftDescription,
      price = newNft.price,
      ownerId = userId,
      creatorId = userId,
      categoryId = newNft.categoryId
    };

    await _nftService.CreateAsync(nft);

    return CreatedAtAction(nameof(Get), new { id = nft.nftId }, nft);
  }

  [HttpPatch("{id:length(24)}")]
  public async Task<IActionResult> Update(string id, NftUpdateDto updatedNft)
  {
    var nft = await _nftService.GetAsync(id);

    if (nft is null)
    {
      return NotFound();
    }

    nft.nftName = updatedNft.nftName ?? nft.nftName;
    nft.nftDescription = updatedNft.nftDescription ?? nft.nftDescription;
    nft.price = updatedNft.price ?? nft.price;
    nft.updatedAt = DateTime.UtcNow;


    await _nftService.UpdateAsync(id, nft);

    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> Delete(string id)
  {
    var nft = await _nftService.GetAsync(id);

    if (nft is null)
    {
      return NotFound();
    }

    await _nftService.RemoveAsync(id);

    return NoContent();
  }

  [HttpGet("creator")]
  public async Task<List<Nft>> GetNftsByCreatorId([FromQuery] string creatorId) =>
      await _nftService.GetNftsByCreatorId(creatorId);

  [HttpGet("owner")]
  public async Task<List<Nft>> GetNftsByOwnerId([FromQuery] string ownerId) =>
      await _nftService.GetNftsByOwnerId(ownerId);

  [HttpGet("with-price-filter")]
  public async Task<List<Nft>> GetNftWithPriceFilter(
  [FromQuery] double? priceMax, [FromQuery] double?
   priceMin, [FromQuery] bool? ascending = true) =>
      await _nftService.GetNftWithPriceFilter(priceMax, priceMin, ascending);

  [HttpGet("search")]
  public async Task<List<Nft>> SearchByNftName([FromQuery] string nftName, [FromQuery] bool ascending = true) =>
      await _nftService.SearchByNftName(nftName, ascending);
}