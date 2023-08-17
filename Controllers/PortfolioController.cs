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
[Authorize]
[Route("api/portfolios/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public PortfolioController(IPortfolioService portfolioService, IProjectService projectService, IMapper mapper) 
    {
        _portfolioService = portfolioService;
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<Portfolio>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _portfolioService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(Portfolio), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var portfolio = await _portfolioService.GetAsync(id);

        if (portfolio is null)
        {
            return NotFound();
        }

        return Ok(portfolio);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PortfolioCreateDto newPortfolio)
    {   
        var userId = HttpContext.Request.Headers["userId"].ToString();

        var existingPortfolio = await _portfolioService.GetPortfolioByUserIdAndProjectId(userId, newPortfolio.projectId);

        var project = await _projectService.GetAsync(newPortfolio.projectId);

        if (project is null)
        {
            return NotFound("Project not found");
        }

        if (existingPortfolio != null && project != null)
        {
            existingPortfolio.amountInvested += newPortfolio.amountInvested;
            project.amountRaised += newPortfolio.amountInvested;
            project.updatedAt = DateTime.UtcNow;
            await _portfolioService.UpdateAsync(existingPortfolio.portfolioId!, existingPortfolio);
            await _projectService.UpdateAsync(project.projectId!, project);
            return Ok(existingPortfolio);
        }

        var portfolio = _mapper.Map<Portfolio>(newPortfolio);
        portfolio.userId = userId;
        await _portfolioService.CreateAsync(portfolio);

        project!.amountRaised += newPortfolio.amountInvested;
        project.noOfInvestors += 1;
        project.updatedAt = DateTime.UtcNow;
        await _projectService.UpdateAsync(project.projectId!, project);


        return CreatedAtAction(nameof(Get), new { id = portfolio.portfolioId }, portfolio);
    }
    
    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Portfolio updatedPortfolio)
    {
        var portfolio = await _portfolioService.GetAsync(id);

        if (portfolio is null)
        {
            return NotFound();
        }

        updatedPortfolio.portfolioId = portfolio.portfolioId;

        await _portfolioService.UpdateAsync(id, updatedPortfolio);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var portfolio = await _portfolioService.GetAsync(id);

        if (portfolio is null)
        {
            return NotFound();
        }

        await _portfolioService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("get-by-userid")]
    [ProducesResponseType(typeof(IList<Portfolio>), 200)]
    public async Task<IActionResult> GetPortfolioByUserId([FromQuery] string userId) =>
        Ok(await _portfolioService.GetPortfolioByUserId(userId));

    [HttpGet("get-by-projectid")]
    [ProducesResponseType(typeof(Portfolio), 200)]
    public async Task<IActionResult> GetPortfolioByProjectId([FromQuery] string projectId) =>
       Ok( await _portfolioService.GetPortfolioByProjectId(projectId));
}