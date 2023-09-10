using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class ProjectControllerTest
{
  private readonly IProjectService _mockProjectService;
  private readonly IProjectUpdateService _mockProjectUpdateService;
  private readonly IProjectDetailService _mockProjectDetailService;
  private readonly IDefaultService<Category> _mockCategoryService;
  private readonly IMapper _mapper;

  public ProjectControllerTest()
  {
    _mockProjectService = A.Fake<IProjectService>();
    _mockProjectUpdateService = A.Fake<IProjectUpdateService>();
    _mockProjectDetailService = A.Fake<IProjectDetailService>();
    _mockCategoryService = A.Fake<IDefaultService<Category>>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetProjects_ReturnsOkResult()
  {
    //Arrange
    var projects = A.Fake<IList<Project>>();

    A.CallTo(() => _mockProjectService.GetAsync()).Returns(Task.FromResult(projects));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Get();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projects);
  }

  [Fact]
  public async Task GetProjectById_ReturnsOkResult()
  {
    //Arrange
    var project = A.Fake<Project>();
    A.CallTo(() => _mockProjectService.GetAsync("1")).Returns(Task.FromResult(project));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);


    //Act
    var result = await controller.Get("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    // okResult.Value.Should().Be(projectDto);
  }

  [Fact]
  public async Task CreateProject_ReturnsCreatedAtActionResult()
  {
    //Arrange

    var projectDto = new ProjectCreateDto
    {
      categoryId = "1",
      projectName = "Test",
      bannerImageUrl = "Test",
      targetAmount = 1000,
      minInvestment = 100,
      noOfDaysLeft = 10,
      projectWalletAddress = "Test",
      customColour = A.Fake<CustomColour>(),
      projectStatus = 0,
      amountRaised = 0,
    };
    var userId = "1";


    var newProject = _mapper.Map<Project>(projectDto);

    A.CallTo(() => _mockProjectService.CreateAsync(newProject)).Returns(Task.FromResult(newProject));
    A.CallTo(() => _mockProjectService.GetProjectByWalletAddress("Test")).Returns(Task.FromResult((Project)null));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);
    controller.ControllerContext.HttpContext = new DefaultHttpContext();
    controller.ControllerContext.HttpContext.Request.Headers["userId"] = userId;

    //Act
    var result = await controller.Post(projectDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdAtActionResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task UpdateProject_ReturnsNoContent()
  {
    //Arrange
    var projectDto = A.Fake<UpdateProjectDto>();
    var project = A.Fake<Project>();

    var updatedProject = _mapper.Map(projectDto, project);

    A.CallTo(() => _mockProjectService.UpdateAsync(project.projectId, updatedProject)).Returns(Task.FromResult(updatedProject));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Update("1", projectDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeleteProject_ReturnsNoContent()
  {
    //Arrange
    var project = A.Fake<Project>();

    A.CallTo(() => _mockProjectService.RemoveAsync("1")).Returns(Task.FromResult(project));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Delete("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetProjectsByUserId_ReturnsOkResult()
  {
    //Arrange
    var projects = A.Fake<IList<Project>>();

    A.CallTo(() => _mockProjectService.GetProjectsByUserId("1")).Returns(Task.FromResult(projects));
    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.GetProjectsByUserId("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projects);
  }

  [Fact]
  public async Task SearchProjects_ReturnsOkResult()
  {
    //Arrange
    var projects = A.Fake<IList<Project>>();

    A.CallTo(() => _mockProjectService.SearchByProjectName("1", true)).Returns(Task.FromResult(projects));

    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.SearchProjects("1", true);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projects);
  }

  [Fact]
  public async Task GetProjectByWalletAddress_ReturnsOkResult()
  {
    //Arrange
    var project = A.Fake<Project>();

    A.CallTo(() => _mockProjectService.GetProjectByWalletAddress("1")).Returns(Task.FromResult(project));

    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.GetProjectByWalletAddress("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task GetProjectWithFilters_ReturnsOkResult()
  {
    //Arrange
    var projects = A.Fake<IList<Project>>();
    var categoryIds = new List<string> { "1" };
    var status = Status.Completed;

    A.CallTo(() => _mockProjectService.GetProjectWithFilters(
      "1",
      false,
      false,
       0,
      false,
      categoryIds,
      1,
      1,
      false,
      1,
      1,
      false,
      1,
      1,
      false,
      1,
      1,
      false,
      false
      )).Returns(Task.FromResult(projects));

    var controller = new ProjectController(_mockProjectService, _mockProjectUpdateService, _mockProjectDetailService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.GetProjectWithFilters(
      "1",
      false,
      false,
      0,
      false,
      categoryIds,
      1,
      1,
      false,
      1,
      1,
      false,
      1,
      1,
      false,
      1,
      1,
      false,
      false
      );

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projects);

  }
}