using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class UserControllerTest 
{
  private readonly IUserService _mockUserService;
  private readonly IPortfolioService _mockPortfolioService;
  private readonly IProjectService _mockProjectService;
  private readonly IUserNftService _mockUserNftService;
  private readonly IProjectLikeService _mockProjectLikeService;
  private readonly INftLikeService _mockNftLikeService;
  private readonly IMapper _mapper;

  // create HttpContext for testing

  public UserControllerTest()
  {
    _mockUserService = A.Fake<IUserService>();
    _mockPortfolioService = A.Fake<IPortfolioService>();
    _mockProjectService = A.Fake<IProjectService>();
    _mockUserNftService = A.Fake<IUserNftService>();
    _mockProjectLikeService = A.Fake<IProjectLikeService>();
    _mockNftLikeService = A.Fake<INftLikeService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetUsers ()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, _mockProjectService, _mockUserNftService, _mockProjectLikeService, _mockNftLikeService, _mapper);
    var users = A.Fake<IList<User>>();
    
    A.CallTo(() => _mockUserService.GetAsync()).Returns(Task.FromResult(users));

    // Act

    var result = await userController.Get();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(users);
  }

  [Fact]
  public async Task GetUserById_ReturnsOkResult()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, _mockProjectService, _mockUserNftService, _mockProjectLikeService, _mockNftLikeService, _mapper);
    var user = A.Fake<User>();
    A.CallTo(() => _mockUserService.GetAsync("1")).Returns(Task.FromResult(user));

    // Act
    var result = await userController.Get("1");

    var portfolio = A.Fake<IList<Portfolio>>();
    A.CallTo(() => _mockPortfolioService.GetPortfolioByUserId("1")).Returns(Task.FromResult(portfolio));
    var nftWatchlist = A.Fake<IList<NftLike>>();
    A.CallTo(() => _mockNftLikeService.GetNftLikesByUserId("1")).Returns(Task.FromResult(nftWatchlist));
    var projectWatchlist = A.Fake<IList<ProjectLike>>();
    A.CallTo(() => _mockProjectLikeService.GetProjectLikesByUserId("1")).Returns(Task.FromResult(projectWatchlist));
    var ownedNfts = A.Fake<IList<UserNft>>();
    A.CallTo(() => _mockUserNftService.GetUserNftByUserId("1")).Returns(Task.FromResult(ownedNfts));
    var listedProjects = A.Fake<IList<Project>>();
    A.CallTo(() => _mockProjectService.GetProjectsByUserId("1")).Returns(Task.FromResult(listedProjects));


    

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task Update_ReturnsNoContentResult()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, _mockProjectService, _mockUserNftService, _mockProjectLikeService, _mockNftLikeService, _mapper);
    userController.ControllerContext.HttpContext = new DefaultHttpContext();
    userController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";    var userDto = A.Fake<UserUpdateDto>();

    var user = A.Fake<User>();
    A.CallTo(() => _mockUserService.GetAsync("1")).Returns(Task.FromResult(user));

    var userUpdate = _mapper.Map(userDto, user);
    A.CallTo(() => _mockUserService.UpdateAsync("1", userUpdate)).Returns(Task.FromResult(userUpdate));

    // Act
    var result = await userController.Update(userDto);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task Delete_ReturnsNoContentResult()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, _mockProjectService, _mockUserNftService, _mockProjectLikeService, _mockNftLikeService, _mapper);
    userController.ControllerContext.HttpContext = new DefaultHttpContext();
    userController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";
    var user = A.Fake<User>();
    A.CallTo(() => _mockUserService.GetAsync("1")).Returns(Task.FromResult(user));

    // Act
    var result = await userController.Delete("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetByWalletAddress_ReturnsOkResult()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, _mockProjectService, _mockUserNftService, _mockProjectLikeService, _mockNftLikeService, _mapper);
    var user = A.Fake<User>();
    A.CallTo(() => _mockUserService.GetUserByWalletAddress("1")).Returns(Task.FromResult(user));

    // Act
    var result = await userController.GetByWalletAddress("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task GetMe_ReturnsOkResult()
  {
    // Arrange
    var userController = new UserController(_mockUserService, _mockPortfolioService, 
    _mockProjectService, _mockUserNftService, _mockProjectLikeService, 
    _mockNftLikeService, _mapper);
    userController.ControllerContext.HttpContext = new DefaultHttpContext();
    userController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";
    var user = A.Fake<User>();
    A.CallTo(() => _mockUserService.GetAsync("1")).Returns(Task.FromResult(user));

    // Act
    var result = await userController.GetMe();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
  }
}