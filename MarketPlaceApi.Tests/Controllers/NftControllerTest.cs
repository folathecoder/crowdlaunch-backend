using Microsoft.AspNetCore.Http;

namespace MARKETPLACEAPI.Tests.Controllers;


public class NftControllerTest
{
  private readonly INftService _mockNftService;
  private readonly IDefaultService<Category> _mockCategoryService;
  private readonly IMapper _mapper;

  public NftControllerTest()
  {
    _mockNftService = A.Fake<INftService>();
    _mockCategoryService = A.Fake<IDefaultService<Category>>();
    _mapper = A.Fake<IMapper>();
  }

  [Fact]
  public async Task GetNfts_ReturnsOkResult()
  {
    //Arrange
    var nfts = A.Fake<IList<Nft>>();

    A.CallTo(() => _mockNftService.GetAsync()).Returns(Task.FromResult(nfts));
    var controller = new NftController(_mockNftService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Get();

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
    okResult.Value.Should().Be(nfts);
  }

  [Fact]
  public async Task GetNftById_ReturnsOkResult()
  {
    //Arrange
    var nft = A.Fake<Nft>();
    A.CallTo(() => _mockNftService.GetAsync("1")).Returns(Task.FromResult(nft));
    var controller = new NftController(_mockNftService, _mockCategoryService, _mapper);

    var nftDto = A.Fake<NftDto>();

    nftDto.nft = nft;
    nftDto.category = A.Fake<Category>();

    //Act
    var result = await controller.Get("1");

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<OkObjectResult>();
    var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    okResult.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task CreateNft_ReturnsCreatedAtActionResult()
  {
    //Arrange
    
    var newNft = A.Fake<NftCreateDto>();

    var nft = _mapper.Map<Nft>(newNft);
    var userId = "1";
  

    A.CallTo(() => _mockNftService.CreateAsync(nft)).Returns(Task.FromResult(nft));
    var controller = new NftController(_mockNftService, _mockCategoryService, _mapper);
    
    controller.ControllerContext.HttpContext = new DefaultHttpContext();
    controller.ControllerContext.HttpContext.Items["User"] = userId;

    

    //Act
    var result = await controller.Post(newNft);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
    createdResult.StatusCode.Should().Be(201);
  }

  [Fact]
  public async Task UpdateNft_ReturnsNoContentResult()
  {
    //Arrange
    var nft = A.Fake<Nft>();
    var nftDto = A.Fake<NftUpdateDto>();
    var userId = "1";


    var nftUpdate = _mapper.Map(nftDto, nft);

    A.CallTo(() => _mockNftService.UpdateAsync(nft.nftId, nftUpdate)).Returns(Task.FromResult(nftUpdate));
    var controller = new NftController(_mockNftService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Update(nft.nftId, nftDto);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);

  }

  [Fact]
  public async Task DeleteNft_ReturnsNoContentResult()
  {
    //Arrange
    var nft = A.Fake<Nft>();
    A.CallTo(() => _mockNftService.RemoveAsync(nft.nftId)).Returns(Task.FromResult(nft));
    var controller = new NftController(_mockNftService, _mockCategoryService, _mapper);

    //Act
    var result = await controller.Delete(nft.nftId);

    //Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<NoContentResult>();
    var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
    noContentResult.StatusCode.Should().Be(204);
  }

}