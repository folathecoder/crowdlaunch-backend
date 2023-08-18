using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class ProjectLikeControllerTest
{
  private readonly IProjectLikeService _mockProjectLikeService;
  private readonly IProjectService _mockProjectService;
  private readonly IMapper _mapper;

  public ProjectLikeControllerTest()
  {
    _mockProjectLikeService = A.Fake<IProjectLikeService>();
    _mockProjectService = A.Fake<IProjectService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetProjectLikes ()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLikes = A.Fake<IList<ProjectLike>>();
    
    A.CallTo(() => _mockProjectLikeService.GetAsync()).Returns(Task.FromResult(projectLikes));

    // Act

    var result = await projectLikeController.Get();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectLikes);
  }

  [Fact]
  public async Task GetProjectLikeById_ReturnsOkResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLike = A.Fake<ProjectLike>();
    A.CallTo(() => _mockProjectLikeService.GetAsync("1")).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.Get("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectLike);
  }

  [Fact]
  public async Task PostProjectLike_ReturnsCreatedAtActionResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    projectLikeController.ControllerContext.HttpContext = new DefaultHttpContext();
    projectLikeController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";
    var projectLikeDto = new ProjectLikeCreateDto 
    {
      projectId = "1",
    };
    var projectLike = _mapper.Map<ProjectLike>(projectLikeDto);
    A.CallTo(() => _mockProjectLikeService.GetProjectLikeByUserIdAndProjectId("1", "1")).Returns(Task.FromResult((ProjectLike)null));
    A.CallTo(() => _mockProjectLikeService.CreateAsync(projectLike)).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.Post(projectLikeDto);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdAtActionResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task DeleteProjectLike_ReturnsNoContentResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    projectLikeController.ControllerContext.HttpContext = new DefaultHttpContext();
    projectLikeController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";
    var projectLike = A.Fake<ProjectLike>();
    A.CallTo(() => _mockProjectLikeService.RemoveAsync("1")).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.Delete("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task UpdateProjectLike_ReturnsNoContentResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLike = A.Fake<ProjectLike>();
    A.CallTo(() => _mockProjectLikeService.UpdateAsync(projectLike.projectLikeId, projectLike)).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.Update(projectLike.projectLikeId, projectLike);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetByProjectId_ReturnsOkResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLike = A.Fake<IList<ProjectLike>>();
    var project = A.Fake<Project>();
    A.CallTo(() => _mockProjectLikeService.GetProjectLikeByProjectId("1")).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.GetByProjectId("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectLike);
  }

  [Fact]
  public async Task GetByUserId_ReturnsOkResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLike = A.Fake<IList<ProjectLike>>();
    A.CallTo(() => _mockProjectLikeService.GetProjectLikesByUserId("1")).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.GetByUserId("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectLike);
  }

  [Fact]
  public async Task GetByUserAndProjectId_ReturnsOkResult()
  {
    // Arrange
    var projectLikeController = new ProjectLikeController(_mockProjectLikeService, _mockProjectService, _mapper);
    var projectLike = A.Fake<ProjectLike>();
    A.CallTo(() => _mockProjectLikeService.GetProjectLikeByUserIdAndProjectId("1", "1")).Returns(Task.FromResult(projectLike));

    // Act
    var result = await projectLikeController.GetByUserIdAndProjectId("1", "1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectLike);
  }
}