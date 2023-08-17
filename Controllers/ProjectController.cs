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
[Route("api/projects/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IProjectUpdateService _projectUpdateService;
    private readonly IProjectDetailService _projectDetailService;
    private readonly IDefaultService<Category> _categoryService;
    private readonly IMapper _mapper;

    public ProjectController(IProjectService projectService, IProjectUpdateService projectUpdateService, 
                            IProjectDetailService projectDetailService, IDefaultService<Category> categoryService, IMapper mapper)
    {
        _projectService = projectService;
        _projectUpdateService = projectUpdateService;
        _projectDetailService = projectDetailService;
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IList<Project>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _projectService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(ProjectDto), 200)]
    public async Task<IActionResult> Get(string id)
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

        var project = _mapper.Map<Project>(newProject);
        project.userId = userId;
        
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

        var projectUpdated = _mapper.Map(updatedProject, project);



        await _projectService.UpdateAsync(id, projectUpdated);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize]
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
    [ProducesResponseType(typeof(IList<Project>), 200)]
    public async Task<IActionResult> GetProjectsByUserId([FromQuery] string userId) =>
        Ok(await _projectService.GetProjectsByUserId(userId));

    [HttpGet("search")]
    [ProducesResponseType(typeof(IList<Project>), 200)]
    public async Task<IActionResult> SearchProjects([FromQuery] string projectName, [FromQuery] bool ascending) =>
        Ok(await _projectService.SearchByProjectName(projectName, ascending));

    [HttpGet("get-by-wallet-address")]
    [ProducesResponseType(typeof(ProjectDto), 200)]
    public async Task<IActionResult> GetProjectByWalletAddress([FromQuery] string walletAddress) {
        var project = await _projectService.GetProjectByWalletAddress(walletAddress);

        if (project is null)
        {
            return NotFound();
        }

        var projectUpdates = await _projectUpdateService.GetProjectUpdatesByProjectId(project.projectId);
        var projectDetail = await _projectDetailService.GetProjectDetailsByProjectId(project.projectId);
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
        

    [HttpGet("get-with-filters")]
    [ProducesResponseType(typeof(IList<Project>), 200)]
    public async Task<IActionResult> GetProjectWithFilters([FromQuery] string? search,
    [FromQuery] bool? newest, [FromQuery] bool? trending, [FromQuery] Status? active,
    [FromQuery] bool? mostLiked, [FromQuery] List<string?> categoryIds, [FromQuery] double? minInvestmentMin,
    [FromQuery] double? minInvestmentMax, [FromQuery] bool? minInvestmentAsc,
    [FromQuery] double? amountRaisedMin, [FromQuery] double? amountRaisedMax, [FromQuery] bool? amountRaisedAsc, 
    [FromQuery] double? targetAmountMin, [FromQuery] double? targetAmountMax, [FromQuery] bool? targetAmountAsc,
    [FromQuery] int? noOfDaysLeftMin, [FromQuery] int? noOfDaysLeftMax, [FromQuery] bool? noOfDaysLeftAsc, 
    [FromQuery] bool? ascending = false) =>
        Ok(await _projectService.GetProjectWithFilters(search,
            newest, trending, active, mostLiked, categoryIds, minInvestmentMin, minInvestmentMax, minInvestmentAsc, 
            amountRaisedMin, amountRaisedMax, amountRaisedAsc, targetAmountMin, targetAmountMax,
            targetAmountAsc, noOfDaysLeftMin, noOfDaysLeftMax, noOfDaysLeftAsc, ascending
        ));
}