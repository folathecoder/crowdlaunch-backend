using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/user/[controller]")]
public class NftController : ControllerBase
{
    private readonly INftService _nftService;
    private readonly IDefaultService<Category> _categoryService;
    private readonly IMapper _mapper;

    public NftController(INftService nftService, IDefaultService<Category> categoryService, IMapper mapper) {
        _nftService = nftService;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<Nft>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _nftService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(NftDto), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var nft = await _nftService.GetAsync(id);
        if (nft is null)
        {
            return NotFound();
        }

        var nftDto = new NftDto
        {
            nft = nft,
            category = await _categoryService.GetAsync(nft.categoryId)
        };
        
        return Ok(nftDto);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(NftCreateDto newNft)
    {   
        var userId = HttpContext.Request.Headers["userId"].ToString();

        var nft = _mapper.Map<Nft>(newNft);
        nft.creatorId = userId;
        nft.ownerId = userId;
        
        await _nftService.CreateAsync(nft);

        return CreatedAtAction(nameof(Get), new { id = nft.nftId }, nft);
    }

    [HttpPatch("{id:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(string id, NftUpdateDto updatedNft)
    {
        var nft = await _nftService.GetAsync(id);

        if (nft is null)
        {
            return NotFound();
        }

        var nftUpdate= _mapper.Map(updatedNft, nft);


        await _nftService.UpdateAsync(id, nftUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
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
    [ProducesResponseType(typeof(IList<Nft>), 200)]
    public async Task<IActionResult> GetNftsByCreatorId([FromQuery] string creatorId) =>
        Ok(await _nftService.GetNftsByCreatorId(creatorId));

    [HttpGet("owner")]
    [ProducesResponseType(typeof(IList<Nft>), 200)]
    public async Task<IActionResult> GetNftsByOwnerId([FromQuery] string ownerId) =>
        Ok(await _nftService.GetNftsByOwnerId(ownerId));

    [HttpGet("with-price-filter")]
    [ProducesResponseType(typeof(IList<Nft>), 200)]
    public async Task<IActionResult> GetNftWithPriceFilter( 
    [FromQuery] double? priceMax, [FromQuery] double?
     priceMin, [FromQuery] bool? ascending = true) =>
        Ok(await _nftService.GetNftWithPriceFilter(priceMax, priceMin, ascending));

    [HttpGet("search")]
    [ProducesResponseType(typeof(IList<Nft>), 200)]
    public async Task<IActionResult> SearchByNftName([FromQuery] string nftName, [FromQuery] bool ascending = true) =>
        Ok(await _nftService.SearchByNftName(nftName, ascending));
}