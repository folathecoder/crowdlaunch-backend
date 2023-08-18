using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class UserNftControllerTest
{
  private readonly IUserNftService _mockUserNftService;
  private readonly IMapper _mapper;

  public UserNftControllerTest()
  {
    _mockUserNftService = A.Fake<IUserNftService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetUserNfts ()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    var userNfts = A.Fake<IList<UserNft>>();
    
    A.CallTo(() => _mockUserNftService.GetAsync()).Returns(Task.FromResult(userNfts));

    // Act

    var result = await userNftController.Get();

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(userNfts);
  }

  [Fact]
  public async Task GetUserNftById_ReturnsOkResult()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    var userNft = A.Fake<UserNft>();
    A.CallTo(() => _mockUserNftService.GetAsync("1")).Returns(Task.FromResult(userNft));

    // Act
    var result = await userNftController.Get("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(userNft);
  }

  [Fact]
  public async Task PostUserNft_ReturnsCreatedAtActionResult()
  {
    // Arrange

    var userNftDto = new UserNftCreateDto
    {
      nftId = "1"
    };
    var userNft = _mapper.Map<UserNft>(userNftDto);

    A.CallTo(() => _mockUserNftService.CreateAsync(userNft)).Returns(Task.FromResult(userNft));
    A.CallTo(() => _mockUserNftService.GetUserNftByUserIdAndNftId("1", "1")).Returns(Task.FromResult((UserNft)null));

    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    userNftController.ControllerContext.HttpContext = new DefaultHttpContext();
    userNftController.ControllerContext.HttpContext.Request.Headers["userId"] = "1";
    // Act
    var result = await userNftController.Post(userNftDto);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdAtActionResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task PutUserNft_ReturnsNoContentResult()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
   
    var userNft = A.Fake<UserNft>();
    A.CallTo(() => _mockUserNftService.GetAsync(userNft.userNftId)).Returns(Task.FromResult(userNft));
    A.CallTo(() => _mockUserNftService.UpdateAsync(userNft.userNftId, userNft)).Returns(Task.FromResult(userNft));
    // Act
    var result = await userNftController.Update(userNft.userNftId, userNft);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeleteUserNft_ReturnsNoContentResult()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    var userNft = A.Fake<UserNft>();
    A.CallTo(() => _mockUserNftService.GetAsync(userNft.userNftId)).Returns(Task.FromResult(userNft));
    A.CallTo(() => _mockUserNftService.RemoveAsync(userNft.userNftId)).Returns(Task.FromResult(userNft));
    // Act
    var result = await userNftController.Delete(userNft.userNftId);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetByUserId_ReturnsOkResult()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    var userNft = A.Fake<IList<UserNft>>();
    A.CallTo(() => _mockUserNftService.GetUserNftByUserId("1")).Returns(Task.FromResult(userNft));

    // Act
    var result = await userNftController.GetByUserId("1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(userNft);
  }

  [Fact]
  public async Task GetByUserIdNftId_ReturnsOkResult()
  {
    // Arrange
    var userNftController = new UserNftController(_mockUserNftService, _mapper);
    var userNft = A.Fake<UserNft>();
    A.CallTo(() => _mockUserNftService.GetUserNftByUserIdAndNftId("1", "1")).Returns(Task.FromResult(userNft));

    // Act
    var result = await userNftController.GetByUserIdNftId("1", "1");

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(userNft);
  }

}

