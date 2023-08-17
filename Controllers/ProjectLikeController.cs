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
[Route("api/project-likes/[controller]")]
public class ProjectLikeController : ControllerBase
{
  private readonly IProjectLikeService _projectLikeService;
  private readonly IProjectService _projectService;
  private readonly IMapper _mapper;

  public ProjectLikeController(IProjectLikeService projectLikeService, IProjectService projectService, IMapper mapper) {
    _projectLikeService = projectLikeService;
    _projectService = projectService;
    _mapper = mapper;
    }
     

    [HttpGet]
    [ProducesResponseType(typeof(IList<ProjectLike>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _projectLikeService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(ProjectLike), 200)]
    public async Task<IActionResult> Get(string id)
    {
    var projectLike = await _projectLikeService.GetAsync(id);

    if (projectLike is null)
    {
        return NotFound();
    }

    return Ok(projectLike);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProjectLikeCreateDto newProjectLike)
    {
    var userId = HttpContext.Request.Headers["userId"].ToString();
    var existingProjectLike = await _projectLikeService.GetProjectLikeByUserIdAndProjectId(
        userId, newProjectLike.projectId);

    if (existingProjectLike is not null) {
        return Conflict("Project Like already exists for this user and project.");
    }

    var projectLike = _mapper.Map<ProjectLike>(newProjectLike);
    projectLike.userId = userId;
    var project = await _projectService.GetAsync(projectLike.projectId);

    if (project is null)
    {
        return NotFound();
    }
    await _projectLikeService.CreateAsync(projectLike);
    

    project.noOfLikes += 1;
    await _projectService.UpdateAsync(projectLike.projectId, project);

    return CreatedAtAction(nameof(Get), new { id = projectLike.projectLikeId }, projectLike);
    }

    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ProjectLike updatedProjectLike)
    {
    var projectLike = await _projectLikeService.GetAsync(id);

    if (projectLike is null)
    {
        return NotFound();
    }

    updatedProjectLike.projectLikeId = projectLike.projectLikeId;

    await _projectLikeService.UpdateAsync(id, updatedProjectLike);

    return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
    var projectLike = await _projectLikeService.GetAsync(id);
    if (projectLike is null)
    {
        return NotFound();
    }
    var project = await _projectService.GetAsync(projectLike.projectId!);

    if (project is null)
    {
        return NotFound("Project not found.");
    }

    project.noOfLikes -= 1;
    await _projectService.UpdateAsync(projectLike.projectId!, project);


    await _projectLikeService.RemoveAsync(id);


    return NoContent();
    }

    [HttpGet("get-by-projectid")]
    [ProducesResponseType(typeof(IList<ProjectLike>), 200)]
    public async Task<IActionResult> GetByProjectId([FromQuery] string projectId) =>
        Ok(await _projectLikeService.GetProjectLikeByProjectId(projectId));

    [HttpGet("get-by-userid")]
    [ProducesResponseType(typeof(IList<ProjectLike>), 200)]
    public async Task<IActionResult> GetByUserId([FromQuery] string userId) =>
        Ok(await _projectLikeService.GetProjectLikesByUserId(userId));

    [HttpGet("get-by-userid-and-projectid")]
    [ProducesResponseType(typeof(ProjectLike), 200)]
    public async Task<IActionResult> GetByUserIdAndProjectId([FromQuery] string userId, [FromQuery] string projectId) {
        var projectLike = await _projectLikeService.GetProjectLikeByUserIdAndProjectId(userId, projectId);
        if (projectLike is null)
        {
            return NotFound();
        }
        return Ok(projectLike);
    }
        
}