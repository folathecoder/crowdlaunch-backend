namespace MARKETPLACEAPI.Tests.Controllers;

public class CategoryControllerTests
{

    private readonly IDefaultService<Category> _mockCategoryService;
  private readonly IMapper _mapper;
  public CategoryControllerTests()
    {
        _mockCategoryService = A.Fake<IDefaultService<Category>>();
        _mapper = A.Fake<IMapper>();
    }

    [Fact]
    public async Task GetCategories_ReturnsOkResult()  
    {
        //Arrange
        var categories = A.Fake<IList<Category>>();
        A.CallTo(() => _mockCategoryService.GetAsync()).Returns(Task.FromResult(categories));
        var controller = new CategoryController(_mockCategoryService, _mapper);
       
        //Act
        var result = await controller.Get();
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(categories);

        
    }

    [Fact]
    public async Task GetCategoryById_ReturnsOkResult()  
    {
        //Arrange
        var category = A.Fake<Category>();
        A.CallTo(() => _mockCategoryService.GetAsync("1")).Returns(Task.FromResult(category));
        var controller = new CategoryController(_mockCategoryService, _mapper);

        //Act
        var result = await controller.Get("1");

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(category);
        
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedAtActionResult()  
    {
        //Arrange
        var categoryDto = A.Fake<CategoryCreateDto>();
        var category = _mapper.Map<Category>(categoryDto);
        A.CallTo(() => _mockCategoryService.CreateAsync(category)).Returns(Task.FromResult(category));

        var controller = new CategoryController(_mockCategoryService, _mapper);

        //Act
        var result = await controller.Post(categoryDto);
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedAtActionResult>();
        var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        okResult.StatusCode.Should().Be(201);
        
    }

    [Fact]
    public async Task UpdateCategory_ReturnsNoContentResult()  
    {
        //Arrange
        var categoryDto = A.Fake<CategoryUpdateDto>();
        var category = A.Fake<Category>();

        var updatedCategory = _mapper.Map(categoryDto, category);
        A.CallTo(() => _mockCategoryService.UpdateAsync(category.categoryId, updatedCategory)).Returns(Task.FromResult(category));

        var controller = new CategoryController(_mockCategoryService, _mapper);

        //Act
        var result = await controller.Update(category.categoryId, categoryDto);


        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        var okResult = result.Should().BeOfType<NoContentResult>().Subject;
        okResult.StatusCode.Should().Be(204);
        
    }

    [Fact]
    public async Task DeleteCategory_ReturnsOkResult()  
    {
        //Arrange
        var category = A.Fake<Category>();
        
        A.CallTo(() => _mockCategoryService.RemoveAsync(category.categoryId)).Returns(Task.FromResult(category));
        var controller = new CategoryController(_mockCategoryService, _mapper);

        //Act
        var result = await controller.Delete(category.categoryId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        var okResult = result.Should().BeOfType<NoContentResult>().Subject;
        okResult.StatusCode.Should().Be(204);
        
    }

}
    