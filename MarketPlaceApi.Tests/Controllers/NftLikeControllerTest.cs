using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;

public class NftLikeControllerTest
{
  private readonly INftLikeService _mockNftLikeService;
  private readonly INftService _mockNftService;
  private readonly IMapper _mapper;

  public NftLikeControllerTest()
  {
    _mockNftLikeService = A.Fake<INftLikeService>();
    _mockNftService = A.Fake<INftService>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetNftLikes_ReturnsOkResult()
  {
    //Arrange
    var nftLikes = A.Fake<IList<NftLike>>();
    A.CallTo(() => _mockNftLikeService.GetAsync()).Returns(Task.FromResult(nftLikes));
    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);

    //Act
    var result = await controller.Get();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(nftLikes);
  }

  [Fact]
  public async Task GetNftLikeById_ReturnsOkResult()
  {
    //Arrange
    var nftLike = A.Fake<NftLike>();
    A.CallTo(() => _mockNftLikeService.GetAsync("1")).Returns(Task.FromResult(nftLike));
    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);

    //Act
    var result = await controller.Get("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(nftLike);
  }

  

  [Fact]
  public async Task PostNftLike_ReturnsCreatedAtActionResult()
  {
      // Arrange
      var nftLikeDto = new NftLikeCreateDto
      {
          nftId = "1"
      };
      var nftLike = _mapper.Map<NftLike>(nftLikeDto);

      // Mock the _nftService.GetAsync method to return a valid nft object
      A.CallTo(() => _mockNftService.GetAsync(nftLike.nftId)).Returns(Task.FromResult(A.Fake<Nft>()));

      // Mock the _nftLikeService.GetNftLikeByUserIdAndNftId to return null
      A.CallTo(() => _mockNftLikeService.GetNftLikeByUserIdAndNftId("1", "1")).Returns(Task.FromResult((NftLike)null));

      // Set up the HttpContext with the required header
      var httpContext = new DefaultHttpContext();
      httpContext.Request.Headers["userId"] = "1";

      var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper)
      {
          ControllerContext = new ControllerContext
          {
              HttpContext = httpContext
          }
      };

    // Act
      var result = await controller.Post(nftLikeDto);

      // Assert
      result.Should().NotBeNull();
      result.Should().BeOfType<CreatedAtActionResult>();
      var createdAtResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
      createdAtResult.StatusCode.Should().Be(201);

  }

  [Fact]
  public async Task UpdateNftLike_ReturnsNoContentResult()
  {
    //Arrange
    var nftLike = A.Fake<NftLike>();
    A.CallTo(() => _mockNftLikeService.UpdateAsync(nftLike.nftId, nftLike)).Returns(Task.FromResult(nftLike));
    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);

    //Act
    var result = await controller.Update("1", nftLike);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task DeleteNftLike_ReturnsNoContentResult()
  {
    //Arrange
    var nftLike = A.Fake<NftLike>();
    A.CallTo(() => _mockNftLikeService.GetAsync("1")).Returns(Task.FromResult(nftLike));
    A.CallTo(() => _mockNftLikeService.RemoveAsync(nftLike.nftId)).Returns(Task.FromResult(nftLike));
    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);
    controller.ControllerContext.HttpContext = new DefaultHttpContext();
    controller.ControllerContext.HttpContext.Request.Headers["userId"] = "2";
    //Act
    var result = await controller.Delete(nftLike.nftId);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var okResult = result.Should().BeOfType<NoContentResult>().Subject;
    okResult.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task GetNftLikesByUserId_ReturnsOkResult()
  {
    //Arrange
    var nftLikes = A.Fake<IList<NftLike>>();
    A.CallTo(() => _mockNftLikeService.GetNftLikesByUserId("1")).Returns(Task.FromResult(nftLikes));

    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);
    

    //Act
    var result = await controller.GetNftLikesByUserId("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(nftLikes);
  }


  [Fact]
  public async Task GetNftLikesByUserAndNftId_ReturnsOkResult()
  {
    //Arrange
    var nftLike = A.Fake<NftLike>();
    A.CallTo(() => _mockNftLikeService.GetNftLikeByUserIdAndNftId("1", "1")).Returns(Task.FromResult(nftLike));

    var controller = new NftLikeController(_mockNftLikeService, _mockNftService, _mapper);

    //Act
    var result = await controller.GetNftLikeByUserIdAndNftId("1", "1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(nftLike);
    
  }
}