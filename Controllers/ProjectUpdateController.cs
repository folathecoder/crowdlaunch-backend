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
[Route("api/project-updates/[controller]")]
public class ProjectUpdateController : ControllerBase
{
    private readonly ProjectUpdateService _projectUpdateService;

    public ProjectUpdateController(ProjectUpdateService projectUpdateService) =>
        _projectUpdateService = projectUpdateService;

    [HttpGet]
    public async Task<List<ProjectUpdate>> Get() =>
        await _projectUpdateService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<ProjectUpdate>> Get(string id)
    {
        var projectUpdate = await _projectUpdateService.GetAsync(id);

        if (projectUpdate is null)
        {
            return NotFound();
        }

        return projectUpdate;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProjectUpdateCreateDto newProjectUpdate)
    {
        var projectUpdate = new ProjectUpdate
        {
            projectId = newProjectUpdate.projectId,
            updateTitle = newProjectUpdate.updateTitle,
            updateMessage = newProjectUpdate.updateMessage
        };
        await _projectUpdateService.CreateAsync(projectUpdate);

        return CreatedAtAction(nameof(Get), new { id = projectUpdate.projectUpdateId }, projectUpdate);
    }
    
    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, ProjectUpdateCreateDto updatedProjectUpdate)
    {

        var projectUpdate = await _projectUpdateService.GetAsync(id);

        if (projectUpdate is null)
        {
            return NotFound();
        }

        projectUpdate.projectId = updatedProjectUpdate.projectId;
        projectUpdate.updateTitle = updatedProjectUpdate.updateTitle;
        projectUpdate.updateMessage = updatedProjectUpdate.updateMessage;
        projectUpdate.updatedAt = DateTime.UtcNow;

        await _projectUpdateService.UpdateAsync(id, projectUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var projectUpdate = await _projectUpdateService.GetAsync(id);

        if (projectUpdate is null)
        {
            return NotFound();
        }

        await _projectUpdateService.RemoveAsync(id);

        return NoContent();
    }
}