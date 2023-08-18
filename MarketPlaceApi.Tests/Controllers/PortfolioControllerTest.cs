using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class PortfolioControllerTest
{
  private readonly IPortfolioService _mockPortfolioService;
  private readonly IProjectService _mockProjectService;
  private readonly IMapper _mapper;

  public PortfolioControllerTest()
  {
    _mockPortfolioService = A.Fake<IPortfolioService>();
    _mockProjectService = A.Fake<IProjectService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetPortfolios_ReturnsOkResult()
  {
    //Arrange
    var portfolios = A.Fake<IList<Portfolio>>();

    A.CallTo(() => _mockPortfolioService.GetAsync()).Returns(Task.FromResult(portfolios));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);

    //Act
    var result = await controller.Get();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(portfolios);
  }

  [Fact]
  public async Task GetPortfolioById_ReturnsOkResult()
  {
    //Arrange
    var portfolio = A.Fake<Portfolio>();
    A.CallTo(() => _mockPortfolioService.GetAsync("1")).Returns(Task.FromResult(portfolio));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);


    //Act
    var result = await controller.Get("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(portfolio);
  }

  [Fact]
  public async Task CreatePortfolio_ReturnsCreatedAtActionResult()
  {
    //Arrange
    var portfolioDto = new PortfolioCreateDto
    {
      projectId = "1",
      status = 0,
      amountInvested = 1000
    };
    var portfolio = A.Fake<Portfolio>();
    var userId = "1";

    var newPortfolio = _mapper.Map<Portfolio>(portfolioDto);

    A.CallTo(() => _mockPortfolioService.CreateAsync(newPortfolio)).Returns(Task.FromResult(newPortfolio));
    A.CallTo(() => _mockPortfolioService.GetPortfolioByUserIdAndProjectId(userId, portfolioDto.projectId)).Returns(Task.FromResult((Portfolio)null));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);
    controller.ControllerContext.HttpContext = new DefaultHttpContext();
    controller.ControllerContext.HttpContext.Request.Headers["userId"] = userId;


    //Act
    var result = await controller.Post(portfolioDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdResult.StatusCode.Should().Be(201);
  }


  [Fact]
  public async Task UpdatePortfolio_ReturnsNoContentResult()
  {
    //Arrange
    var portfolio = A.Fake<Portfolio>();
    A.CallTo(() => _mockPortfolioService.UpdateAsync(portfolio.portfolioId, portfolio)).Returns(Task.FromResult(portfolio));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);

    //Act
    var result = await controller.Update(portfolio.portfolioId, portfolio);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeletePortfolio_ReturnsNoContentResult()
  {
    //Arrange
    var portfolio = A.Fake<Portfolio>();
    A.CallTo(() => _mockPortfolioService.RemoveAsync(portfolio.portfolioId)).Returns(Task.FromResult(portfolio));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);

    //Act
    var result = await controller.Delete(portfolio.portfolioId);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetPortfolioByUserId_ReturnsOkResult()
  {
    //Arrange
    var portfolio = A.Fake<IList<Portfolio>>();

    A.CallTo(() => _mockPortfolioService.GetPortfolioByUserId("1")).Returns(Task.FromResult(portfolio));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);
    
    //Act
    var result = await controller.GetPortfolioByUserId("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(portfolio);
   
  }

  [Fact]
  public async Task GetPortfolioByProjectId_ReturnsOkResult()
  {
    //Arrange
    var portfolio = A.Fake<Portfolio>();
    A.CallTo(() => _mockPortfolioService.GetPortfolioByProjectId("1")).Returns(Task.FromResult(portfolio));
    var controller = new PortfolioController(_mockPortfolioService, _mockProjectService, _mapper);
    
    
    //Act
    var result = await controller.GetPortfolioByProjectId("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(portfolio);
  }
}