using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PlainBlog.Post.Abstractions;
using PlainBlog.Post.Api;
using PlainBlog.Post.Api.Extensions;
using Xunit;

namespace PlainBlog.Post.UnitTests;

[Trait("Category", nameof(PostControllerTests))]
public class PostControllerTests
{
    private readonly PostController _controller;
    private readonly Mock<ILogger<PostController>> _loggertMock = new();
    private readonly Mock<IPostManagementService> _postManagementServiceMock = new();
    private static readonly Abstractions.Post[] _posts = new[]
        {
            new Abstractions.Post(){ Id = 1, Title ="Title1", Description="Description1", Content="Content1", AuthorId= 1 },
            new Abstractions.Post(){ Id = 2, Title ="Title2", Description="Description2", Content="Content2", AuthorId= 1 },
            new Abstractions.Post(){ Id = 3, Title ="Title3", Description="Description3", Content="Content3", AuthorId= 1 },
            new Abstractions.Post(){ Id = 4, Title ="Title4", Description="Description4", Content="Content4", AuthorId= 1 },
        };

    public PostControllerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddPostProviders();
        serviceCollection.AddSingleton(_loggertMock.Object);
        serviceCollection.AddSingleton(_postManagementServiceMock.Object);
        serviceCollection.BuildServiceProvider();

        _controller = new PostController(_postManagementServiceMock.Object, _loggertMock.Object);
    }

    [Fact]
    public async Task GetAsyncPosts_Success_Returns200()
    {
        // Arrange
        _postManagementServiceMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_posts);

        // Act
        var result = await _controller.GetAsync(CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAsyncPost_Success_Returns200()
    {
        // Arrange
        int postId = 2;
        _postManagementServiceMock.Setup(x => x.GetAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_posts[1]);

        // Act
        var result = await _controller.GetAsync(postId, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAsyncPost_NotFound_Returns404()
    {
        // Arrange
        int postId = 20;
        _postManagementServiceMock.Setup(x => x.GetAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Abstractions.Post?)null);

        // Act
        var result = await _controller.GetAsync(postId, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateAsync_Success_Returns200()
    {
        // Arrange
        var model = new PostSave()
        {
            AuthorId = 1,
            Content = "Content5",
            Description = "Description5",
            Title = "Title5"
        };
        _postManagementServiceMock.Setup(x => x.CreateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        // Act
        var result = await _controller.CreateAsync(model, CancellationToken.None);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateAsync_ValidationException_Returns404()
    {
        // Arrange
        var model = new PostSave()
        {
            AuthorId = 1,
            Content = "",
            Description = "Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description5Description",
            Title = ""
        };
        _postManagementServiceMock.Setup(s => s.CreateAsync(It.IsAny<PostSave>(), It.IsAny<CancellationToken>())).ThrowsAsync(new FluentValidation.ValidationException("error"));

        // Act
        var result = await _controller.CreateAsync(model, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}