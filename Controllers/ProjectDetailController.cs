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
[Route("api/project-details/[controller]")]
public class ProjectDetailController : ControllerBase
{
    private readonly IProjectDetailService _projectDetailService;
    private readonly IMapper _mapper;

    public ProjectDetailController(IProjectDetailService projectDetailService, IMapper mapper) {
        _projectDetailService = projectDetailService;
        _mapper = mapper;
    }
        

    [HttpGet]
    [ProducesResponseType(typeof(IList<ProjectDetail>), 200)]
    public async Task<IActionResult> Get() =>
        Ok(await _projectDetailService.GetAsync());

    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(typeof(ProjectDetail), 200)]
    public async Task<IActionResult> Get(string id)
    {
        var projectDetail = await _projectDetailService.GetAsync(id);

        if (projectDetail is null)
        {
            return NotFound();
        }

        return Ok(projectDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProjectDetailCreateDto newProjectDetail)
    {
        var existingProjectDetail = await _projectDetailService.GetProjectDetailsByProjectId(newProjectDetail.projectId!);

        // if (existingProjectDetail is not null)
        // {
        //     return Conflict("Project Details already exists for this project.");
        // }

    var projectDetail = _mapper.Map<ProjectDetail>(newProjectDetail);

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

        var projectDetailUpdate = _mapper.Map(updatedProjectDetail, projectDetail);

        await _projectDetailService.UpdateAsync(id, projectDetailUpdate);

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