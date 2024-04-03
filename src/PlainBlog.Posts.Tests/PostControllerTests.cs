using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PlainBlog.Posts.Api;

namespace PlainBlog.Posts.Tests;

[Trait("Category", nameof(PostControllerTests))]
public class PostControllerTests
{
    private readonly PostController _controller;
    private readonly Mock<ILogger<PostController>> _loggertMock = new();

    public PostControllerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_loggertMock.Object);
        serviceCollection.BuildServiceProvider();

        _controller = new PostController(_loggertMock.Object);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPosts()
    {
        // Arrange

        // Act
        var result = await _controller.GetAsync(CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}