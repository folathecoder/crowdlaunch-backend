using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class ProjectDetailControllerTest
{
  private readonly IProjectDetailService _mockProjectDetailService;
  private readonly IMapper _mapper;

  public ProjectDetailControllerTest()
  {
    _mockProjectDetailService = A.Fake<IProjectDetailService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetProjectDetails_ReturnsOkResult()
  {
    //Arrange
    var projectDetails = A.Fake<IList<ProjectDetail>>();

    A.CallTo(() => _mockProjectDetailService.GetAsync()).Returns(Task.FromResult(projectDetails));
    var controller = new ProjectDetailController(_mockProjectDetailService, _mapper);

    //Act
    var result = await controller.Get();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectDetails);
  }

  [Fact]
  public async Task GetProjectDetailById_ReturnsOkResult()
  {
    //Arrange
    var projectDetail = A.Fake<ProjectDetail>();
    A.CallTo(() => _mockProjectDetailService.GetAsync("1")).Returns(Task.FromResult(projectDetail));
    var controller = new ProjectDetailController(_mockProjectDetailService, _mapper);

    //Act
    var result = await controller.Get("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(projectDetail);
  }

  [Fact]
  public async Task PostProjectDetail_ReturnsCreatedAtActionResult()
  {
    //Arrange
    var projectDetailDto = new ProjectDetailCreateDto
    {
      projectId = "1",
      overview = "overview",
      competitors = "competitors",
      strategy = "strategy",
      financials = "financials",
      dividend = "dividends",
      risks = "risks",
      performance = "performance",
    };
    var projectDetail = _mapper.Map<ProjectDetail>(projectDetailDto);
    A.CallTo(() => _mockProjectDetailService.GetProjectDetailsByProjectId("1")).Returns(Task.FromResult((ProjectDetail)null));
    A.CallTo(() => _mockProjectDetailService.CreateAsync(projectDetail)).Returns(Task.FromResult(projectDetail));
    var controller = new ProjectDetailController(_mockProjectDetailService, _mapper);

    //Act
    var result = await controller.Post(projectDetailDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    okResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task UpdateProjectDetail_ReturnsNoContentResult()
  {
    //Arrange
    var projectDetail = A.Fake<ProjectDetail>();
    var projectDetailDto = A.Fake<ProjectDetailCreateDto>();

    var updatedProjectDetail = _mapper.Map(projectDetailDto, projectDetail);
    A.CallTo(() => _mockProjectDetailService.UpdateAsync(projectDetail.projectDetailId, updatedProjectDetail)).Returns(Task.FromResult(updatedProjectDetail));
    var controller = new ProjectDetailController(_mockProjectDetailService, _mapper);

    //Act
    var result = await controller.Update(projectDetail.projectDetailId, projectDetailDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeleteProjectDetail_ReturnsNoContentResult()
  {
    //Arrange
    var projectDetail = A.Fake<ProjectDetail>();
    A.CallTo(() => _mockProjectDetailService.RemoveAsync(projectDetail.projectDetailId)).Returns(Task.FromResult(projectDetail));
    var controller = new ProjectDetailController(_mockProjectDetailService, _mapper);

    //Act
    var result = await controller.Delete(projectDetail.projectDetailId);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }
}