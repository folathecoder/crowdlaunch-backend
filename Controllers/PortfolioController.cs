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
[Route("api/portfolios/[controller]")]
public class PortfolioController : ControllerBase
{
  private readonly PortfolioService _portfolioService;
  private readonly ProjectService _projectService;

  public PortfolioController(PortfolioService portfolioService, ProjectService projectService)
  {
    _portfolioService = portfolioService;
    _projectService = projectService;
  }

  [HttpGet]
  public async Task<List<Portfolio>> Get() =>
      await _portfolioService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<Portfolio>> Get(string id)
  {
    var portfolio = await _portfolioService.GetAsync(id);

    if (portfolio is null)
    {
      return NotFound();
    }

    return portfolio;
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

    var portfolio = new Portfolio
    {
      projectId = newPortfolio.projectId,
      userId = userId,
      status = newPortfolio.status,
      amountInvested = newPortfolio.amountInvested,
      investmentDate = newPortfolio.investmentDate
    };
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
  public async Task<List<Portfolio>> GetPortfolioByUserId([FromQuery] string userId) =>
      await _portfolioService.GetPortfolioByUserId(userId);

  [HttpGet("get-by-projectid")]
  public async Task<ActionResult<Portfolio?>> GetPortfolioByProjectId([FromQuery] string projectId) =>
      await _portfolioService.GetPortfolioByProjectId(projectId);
}