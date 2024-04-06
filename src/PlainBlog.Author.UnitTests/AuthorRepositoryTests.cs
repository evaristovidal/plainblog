using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using PlainBlog.Author.Repository;
using PlainBlog.Store;
using Xunit;

namespace PlainBlog.Author.UnitTests;

[Trait("Category", nameof(AuthorRepositoryTests))]
public class AuthorRepositoryTests
{
    private readonly IAuthorRepository _repository;
    private readonly Mock<IServiceProvider> _serviceProviderMock = new();
    private readonly Mock<PlainBlogContext> _contextMock;

    private readonly List<PlainBlog.Store.Entities.Author> _authors;

    public AuthorRepositoryTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_serviceProviderMock.Object);
        serviceCollection.BuildServiceProvider();

        var dbContextOptions = (new DbContextOptionsBuilder<PlainBlogContext>()).Options;
        _contextMock = new Mock<PlainBlogContext>(dbContextOptions);
        _repository = new AuthorRepository(_contextMock.Object);

        _authors = new List<Store.Entities.Author>()
        {
            new (){ Id = 1, Name ="Name1", Surname="Surname1" },
            new (){ Id = 2, Name ="Name2" },
            new (){ Id = 3, Name ="Name3", Surname="Surname1" },
        };
    }

    [Fact]
    public async Task GetAsync_Returns_Correct_Authors()
    {
        // Arrange
        var authorId = 1;
        _contextMock.Setup<DbSet<Store.Entities.Author>>(x => x.Authors)
                    .ReturnsDbSet(_authors);

        // Act
        var result = await _repository.GetAsync(authorId, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_authors.ToList()[0].Name, result.Name);
        Assert.Equal(_authors.ToList()[0].Id, result.Id);
    }

    [Fact]
    public async Task GetAsync_WhenAuthorsNotFound_ShouldReturnEmptyList()
    {
        // Arrange
        var authorId = 10;
        _contextMock.Setup<DbSet<Store.Entities.Author>>(x => x.Authors)
                    .ReturnsDbSet(_authors);

        // Act
        var result = await _repository.GetAsync(authorId, It.IsAny<CancellationToken>());

        // Assert
        Assert.Null(result);
    }
}
