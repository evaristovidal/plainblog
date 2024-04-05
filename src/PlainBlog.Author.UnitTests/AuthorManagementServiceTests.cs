using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PlainBlog.Author.Abstractions;
using PlainBlog.Author.Extensions;
using PlainBlog.Author.Repository;
using Xunit;

namespace PlainBlog.Author.UnitTests;

[Trait("Category", nameof(AuthorManagementServiceTests))]
public class AuthorManagementServiceTests
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly Mock<ILogger<AuthorManagementService>> _loggertMock;
    private readonly IAuthorManagementService _service;

    private static readonly Abstractions.Author[] _authors = new Abstractions.Author[]
        {
            new (){ Id = 1, Name ="Name1", Surname="Description1" },
            new (){ Id = 2, Name ="Name2" },
            new (){ Id = 3, Name ="Name3", Surname="Description1" },
        };

    public AuthorManagementServiceTests()
    {
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _loggertMock = new Mock<ILogger<AuthorManagementService>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLocalization();
        serviceCollection.AddLogging();
        serviceCollection.AddAuthorCore();
        serviceCollection.AddSingleton(_authorRepositoryMock.Object);
        serviceCollection.AddSingleton(_loggertMock.Object);
        var services = serviceCollection.BuildServiceProvider();

        _service = services.GetRequiredService<IAuthorManagementService>();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnAuthors()
    {
        // Arrange
        var  authorId = 1;
        _authorRepositoryMock.Setup(x => x.GetAsync(authorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_authors[0]);

        // Act
        var result = await _service.GetAsync(authorId, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_authors.ToList()[0].Name, result.Name);
        Assert.Equal(_authors.ToList()[0].Id, result.Id);
        _authorRepositoryMock.Verify(x => x.GetAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
