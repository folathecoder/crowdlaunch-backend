using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class ProjectUpdateControllerTest
{
  private readonly IProjectUpdateService _mockProjectUpdateService;
  private readonly IMapper _mapper;

  public ProjectUpdateControllerTest()
  {
    _mockProjectUpdateService = A.Fake<IProjectUpdateService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetProjectUpdates ()
  {
    // Arrange
    var projectUpdateController = new ProjectUpdateController(_mockProjectUpdateService, _mapper);
    var projectUpdates = A.Fake<IList<ProjectUpdate>>();
    
    A.CallTo(() => _mockProjectUpdateService.GetAsync()).Returns(Task.FromResult(projectUpdates));

    // Act

    var result = await projectUpdateController.Get();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectUpdates);
  }

  [Fact]
  public async Task GetProjectUpdateById_ReturnsOkResult()
  {
    // Arrange
    var projectUpdateController = new ProjectUpdateController(_mockProjectUpdateService, _mapper);
    var projectUpdate = A.Fake<ProjectUpdate>();
    A.CallTo(() => _mockProjectUpdateService.GetAsync("1")).Returns(Task.FromResult(projectUpdate));

    // Act
    var result = await projectUpdateController.Get("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectUpdate);
  }

  [Fact]
  public async Task PostProjectUpdate_ReturnsCreatedAtActionResult()
  {
    // Arrange
    var projectUpdateController = new ProjectUpdateController(_mockProjectUpdateService, _mapper);
    
    var projectUpdateDto = A.Fake<ProjectUpdateCreateDto>();
    var projectUpdate = _mapper.Map<ProjectUpdate>(projectUpdateDto);
    A.CallTo(() => _mockProjectUpdateService.CreateAsync(projectUpdate)).Returns(Task.FromResult(projectUpdate));

    // Act
    var result = await projectUpdateController.Post(projectUpdateDto);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdAtActionResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task UpdateProjectUpdate_ReturnsNoContentResult()
  {
    // Arrange
    var projectUpdateController = new ProjectUpdateController(_mockProjectUpdateService, _mapper);
    var projectUpdate = A.Fake<ProjectUpdate>();
    var projectUpdateDto = A.Fake<ProjectUpdateCreateDto>();

    var updatedProjectUpdate = _mapper.Map(projectUpdateDto, projectUpdate);
    A.CallTo(() => _mockProjectUpdateService.UpdateAsync(projectUpdate.projectUpdateId, updatedProjectUpdate )).Returns(Task.FromResult(updatedProjectUpdate));

    // Act
    var result = await projectUpdateController.Update(projectUpdate.projectUpdateId, projectUpdateDto);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeleteProjectUpdate_ReturnsNoContentResult()
  {
    // Arrange
    var projectUpdateController = new ProjectUpdateController(_mockProjectUpdateService, _mapper);
    var projectUpdate = A.Fake<ProjectUpdate>();
    A.CallTo(() => _mockProjectUpdateService.RemoveAsync("1")).Returns(Task.FromResult(projectUpdate));

    // Act
    var result = await projectUpdateController.Delete("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }
  
}