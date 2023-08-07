using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/projects/[controller]")]
public class ProjectController : ControllerBase
{
  private readonly ProjectService _projectService;
  private readonly ProjectUpdateService _projectUpdateService;
  private readonly ProjectDetailService _projectDetailService;
  private readonly CategoryService _categoryService;

  public ProjectController(ProjectService projectService, ProjectUpdateService projectUpdateService,
                          ProjectDetailService projectDetailService, CategoryService categoryService)
  {
    _projectService = projectService;
    _projectUpdateService = projectUpdateService;
    _projectDetailService = projectDetailService;
    _categoryService = categoryService;
  }

  [HttpGet]
  public async Task<List<Project>> Get() =>
      await _projectService.GetAsync();

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<ProjectDto>> Get(string id)
  {
    var project = await _projectService.GetAsync(id);

    if (project is null)
    {
      return NotFound();
    }

    var projectUpdates = await _projectUpdateService.GetProjectUpdatesByProjectId(id);
    var projectDetail = await _projectDetailService.GetProjectDetailsByProjectId(id);
    var category = await _categoryService.GetAsync(project.categoryId!);

    var projectDto = new ProjectDto
    {
      project = project,
      projectUpdates = projectUpdates,
      projectDetails = projectDetail,
      category = category
    };

    return Ok(projectDto);
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Post(ProjectCreateDto newProject)
  {
    var userId = HttpContext.Request.Headers["userId"].ToString();

    var existingProject = await _projectService.GetProjectByWalletAddress(newProject.projectWalletAddress);

    if (existingProject is not null)
    {
      return BadRequest("Project with this wallet address already exists");
    }

    var project = new Project
    {
      userId = userId,
      categoryId = newProject.categoryId,
      projectName = newProject.projectName,
      bannerImageUrl = newProject.bannerImageUrl,
      targetAmount = newProject.targetAmount,
      minInvestment = newProject.minInvestment,
      noOfDaysLeft = newProject.noOfDaysLeft,
      projectWalletAddress = newProject.projectWalletAddress,
      customColour = newProject.customColour,
      projectStatus = newProject.projectStatus,
      amountRaised = newProject.amountRaised
    };

    await _projectService.CreateAsync(project);

    return CreatedAtAction(nameof(Get), new { id = project.projectId }, project);
  }

  [HttpPatch("{id:length(24)}")]
  [Authorize]
  public async Task<IActionResult> Update(string id, UpdateProjectDto updatedProject)
  {

    var project = await _projectService.GetAsync(id);

    if (project is null)
    {
      return NotFound();
    }

    project.projectName = updatedProject.projectName ?? project.projectName;
    project.bannerImageUrl = updatedProject.bannerImageUrl ?? project.bannerImageUrl;
    project.targetAmount = updatedProject.targetAmount;
    project.minInvestment = updatedProject.minInvestment;
    project.noOfDaysLeft = updatedProject.noOfDaysLeft;
    project.projectWalletAddress = updatedProject.projectWalletAddress ?? project.projectWalletAddress;
    project.customColour = updatedProject.customColour ?? project.customColour;
    project.projectStatus = updatedProject.projectStatus;
    project.amountRaised = updatedProject.amountRaised;
    project.categoryId = updatedProject.categoryId ?? project.categoryId;
    project.updatedAt = DateTime.UtcNow;



    await _projectService.UpdateAsync(id, project);

    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> Delete(string id)
  {
    var project = await _projectService.GetAsync(id);

    if (project is null)
    {
      return NotFound();
    }

    await _projectService.RemoveAsync(id);

    return NoContent();
  }

  [HttpGet("get-by-userid")]
  public async Task<List<Project>> GetProjectsByUserId([FromQuery] string userId) =>
      await _projectService.GetProjectsByUserId(userId);

  [HttpGet("search")]
  public async Task<List<Project>> SearchProjects([FromQuery] string projectName, [FromQuery] bool ascending) =>
      await _projectService.SearchByProjectName(projectName, ascending);

  [HttpGet("get-by-wallet-address")]
  public async Task<ActionResult<Project?>> GetProjectByWalletAddress([FromQuery] string walletAddress)
  {
    var project = await _projectService.GetProjectByWalletAddress(walletAddress);

    if (project is null)
    {
      return NotFound();
    }

    return project;
  }


  [HttpGet("get-with-filters")]
  public async Task<List<Project>> GetProjectWithFilters([FromQuery] string? search,
  [FromQuery] bool? newest, [FromQuery] bool? trending, [FromQuery] Status? active,
  [FromQuery] bool? mostLiked, [FromQuery] List<string?> categoryIds, [FromQuery] double? minInvestmentMin,
  [FromQuery] double? minInvestmentMax, [FromQuery] bool? minIvestmentAsc,
  [FromQuery] double? amountRaisedMin, [FromQuery] double? amountRaisedMax, [FromQuery] bool? amountRaisedAsc,
  [FromQuery] double? targetAmountMin, [FromQuery] double? targetAmountMax, [FromQuery] bool? targetAmountAsc,
  [FromQuery] int? noOfDaysLeftMin, [FromQuery] int? noOfDaysLeftMax, [FromQuery] bool? noOfDaysLeftAsc,
  [FromQuery] bool? ascending = false) =>
      await _projectService.GetProjectWithFilters(search,
          newest, trending, active, mostLiked, categoryIds, minInvestmentMin, minInvestmentMax, minIvestmentAsc,
          amountRaisedMin, amountRaisedMax, amountRaisedAsc, targetAmountMin, targetAmountMax,
          targetAmountAsc, noOfDaysLeftMin, noOfDaysLeftMax, noOfDaysLeftAsc, ascending
      );
}