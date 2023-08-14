using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;
using MARKETPLACEAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARKETPLACEAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/project-likes/[controller]")]
public class ProjectLikeController : ControllerBase
{
    private readonly ProjectLikeService _projectLikeService;
    private readonly ProjectService _projectService;

    public ProjectLikeController(ProjectLikeService projectLikeService, ProjectService projectService)
    {
        _projectLikeService = projectLikeService;
        _projectService = projectService;
    }


    [HttpGet]
    public async Task<List<ProjectLike>> Get() =>
        await _projectLikeService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ProjectLike>> Get(string id)
    {
        var projectLike = await _projectLikeService.GetAsync(id);

        if (projectLike is null)
        {
            return NotFound();
        }

        return projectLike;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(ProjectLikeCreateDto newProjectLike)
    {
        var userId = HttpContext.Request.Headers["userId"].ToString();
        var existingProjectLike = await _projectLikeService.GetProjectLikeByUserIdAndProjectId(
            userId, newProjectLike.projectId);

        if (existingProjectLike is not null)
        {
            return Conflict("Project Like already exists for this user and project.");
        }

        var projectLike = new ProjectLike
        {
            projectId = newProjectLike.projectId,
            userId = userId
        };
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
    [Authorize]
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
    [Authorize]
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
    public async Task<List<ProjectLike>> GetByProjectId([FromQuery] string projectId) =>
        await _projectLikeService.GetProjectLikeByProjectId(projectId);

    [HttpGet("get-by-userid")]
    public async Task<List<ProjectLike>> GetByUserId([FromQuery] string userId) =>
        await _projectLikeService.GetProjectLikesByUserId(userId);

    [HttpGet("get-by-userid-and-projectid")]
    public async Task<ActionResult<ProjectLike?>> GetByUserIdAndProjectId([FromQuery] string userId, [FromQuery] string projectId)
    {
        var projectLike = await _projectLikeService.GetProjectLikeByUserIdAndProjectId(userId, projectId);
        if (projectLike is null)
        {
            return NotFound();
        }
        return projectLike;
    }

}