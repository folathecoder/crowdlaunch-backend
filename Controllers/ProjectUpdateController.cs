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
[Route("api/project-updates/[controller]")]
public class ProjectUpdateController : ControllerBase
{
    private readonly IProjectUpdateService _projectUpdateService;
    private readonly IMapper _mapper;

    public ProjectUpdateController(IProjectUpdateService projectUpdateService, IMapper mapper) {
        _projectUpdateService = projectUpdateService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ProjectUpdate>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _projectUpdateService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(ProjectUpdate), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var projectUpdate = await _projectUpdateService.GetAsync(id);

        if (projectUpdate is null)
        {
            return NotFound();
        }

        return Ok(projectUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProjectUpdateCreateDto newProjectUpdate)
    {
        var projectUpdate = _mapper.Map<ProjectUpdate>(newProjectUpdate);
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

        var projectUpdateUpdate = _mapper.Map(updatedProjectUpdate, projectUpdate);
        projectUpdateUpdate.updatedAt = DateTime.UtcNow;

        await _projectUpdateService.UpdateAsync(id, projectUpdateUpdate);

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