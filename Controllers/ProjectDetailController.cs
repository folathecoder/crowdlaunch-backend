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
[Route("api/project-details/[controller]")]
public class ProjectDetailController : ControllerBase
{
  private readonly ProjectDetailService _projectDetailService;

  public ProjectDetailController(ProjectDetailService projectDetailService) =>
      _projectDetailService = projectDetailService;

  [HttpGet]
  public async Task<List<ProjectDetail>> Get() =>
      await _projectDetailService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<ProjectDetail>> Get(string id)
  {
    var projectDetail = await _projectDetailService.GetAsync(id);

    if (projectDetail is null)
    {
      return NotFound();
    }

    return projectDetail;
  }

  [HttpPost]
  public async Task<IActionResult> Post(ProjectDetailCreateDto newProjectDetail)
  {
    var existingProjectDetail = await _projectDetailService.GetProjectDetailsByProjectId(newProjectDetail.projectId!);

    if (existingProjectDetail is not null)
    {
      return Conflict("Project Details already exists for this project.");
    }

    var projectDetail = new ProjectDetail
    {
      projectId = newProjectDetail.projectId,
      overview = newProjectDetail.overview,
      competitors = newProjectDetail.competitors,
      strategy = newProjectDetail.strategy,
      financials = newProjectDetail.financials,
      dividend = newProjectDetail.dividend,
      risks = newProjectDetail.risks,
      performance = newProjectDetail.performance
    };

    await _projectDetailService.CreateAsync(projectDetail);

    return CreatedAtAction(nameof(Get), new { id = projectDetail.projectDetailId }, projectDetail);
  }

  [HttpPatch("{id:length(24)}")]
  public async Task<IActionResult> Update(string id, ProjectDetailCreateDto updatedProjectDetail)
  {
    var projectDetail = await _projectDetailService.GetAsync(id);

    if (projectDetail is null)
    {
      return NotFound();
    }

    projectDetail.projectId = updatedProjectDetail.projectId ?? projectDetail.projectId;
    projectDetail.overview = updatedProjectDetail.overview ?? projectDetail.overview;
    projectDetail.competitors = updatedProjectDetail.competitors ?? projectDetail.competitors;
    projectDetail.strategy = updatedProjectDetail.strategy ?? projectDetail.strategy;
    projectDetail.financials = updatedProjectDetail.financials ?? projectDetail.financials;
    projectDetail.dividend = updatedProjectDetail.dividend ?? projectDetail.dividend;
    projectDetail.risks = updatedProjectDetail.risks ?? projectDetail.risks;
    projectDetail.performance = updatedProjectDetail.performance ?? projectDetail.performance;
    projectDetail.updatedAt = DateTime.UtcNow;

    await _projectDetailService.UpdateAsync(id, projectDetail);

    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> Delete(string id)
  {
    var projectDetail = await _projectDetailService.GetAsync(id);

    if (projectDetail is null)
    {
      return NotFound();
    }

    await _projectDetailService.RemoveAsync(id);

    return NoContent();
  }
}