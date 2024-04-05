using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PlainBlog.Author.Abstractions;
using PlainBlog.Post.Abstractions;
using PlainBlog.Post.Extensions;
using PlainBlog.Post.Repository;
using PlainBlog.Store.Entities;
using Xunit;

namespace PlainBlog.Post.UnitTests;

[Trait("Category", nameof(PostManagementServiceTests))]
public class PostManagementServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IAuthorManagementService> _authorManagementServiceMock;
    private readonly Mock<ILogger<PostManagementService>> _loggertMock;
    private readonly Mock<IValidator<PostSave>> _validatorMock;
    private readonly IPostManagementService _service;

    private static readonly Abstractions.Post[] _posts = new[]
        {
            new Abstractions.Post(){ Id = 1, Title ="Title1", Description="Description1", Content="Content1", AuthorId= 1 },
            new Abstractions.Post(){ Id = 2, Title ="Title2", Description="Description2", Content="Content2", AuthorId= 1 },
            new Abstractions.Post(){ Id = 3, Title ="Title3", Description="Description3", Content="Content3", AuthorId= 1 },
            new Abstractions.Post(){ Id = 4, Title ="Title4", Description="Description4", Content="Content4", AuthorId= 1 },
        };

    public PostManagementServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _authorManagementServiceMock = new Mock<IAuthorManagementService>();
        _loggertMock = new Mock<ILogger<PostManagementService>>();
        _validatorMock = new Mock<IValidator<PostSave>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLocalization();
        serviceCollection.AddLogging();
        serviceCollection.AddPostCore();
        serviceCollection.AddSingleton(_authorManagementServiceMock.Object);
        serviceCollection.AddSingleton(_postRepositoryMock.Object);
        serviceCollection.AddSingleton(_loggertMock.Object);
        serviceCollection.AddSingleton(_validatorMock.Object);
        var services = serviceCollection.BuildServiceProvider();

        _service = services.GetRequiredService<IPostManagementService>();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPosts()
    {
        // Arrange
        _postRepositoryMock.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_posts);

        // Act
        var result = await _service.GetAsync(It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(_posts, result);
        _postRepositoryMock.Verify(x => x.GetAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPost_WhenIdReceviedAndPostExists_WithoutAuthor()
    {
        // Arrange
        var postId = 2;
        var cancellationToken = CancellationToken.None;
        var author = new Author.Abstractions.Author() { Id = 1, Name = "Name1", Surname = "Surname1" };
        _postRepositoryMock.Setup(x => x.GetAsync(postId, cancellationToken))
            .ReturnsAsync(_posts[1]);
        _authorManagementServiceMock.Setup(x => x.GetAsync(_posts[1].AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        var result = await _service.GetAsync(postId, false, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_posts[1], result);
        Assert.Null(result.Author);
        _postRepositoryMock.Verify(x => x.GetAsync(postId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPost_WhenIdReceviedAndPostExists_WithAuthor()
    {
        // Arrange
        var postId = 2;
        var author = new Author.Abstractions.Author() { Id = 1, Name = "Name1", Surname = "Surname1" };
        _postRepositoryMock.Setup(x => x.GetAsync(postId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_posts[1]);
        _authorManagementServiceMock.Setup(x => x.GetAsync(_posts[1].AuthorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        var result = await _service.GetAsync(postId, true, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_posts[1], result);
        Assert.NotNull(result.Author);
        _postRepositoryMock.Verify(x => x.GetAsync(postId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenModelIsNull()
    {
        // Arrange and Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowValidationException_WhenModelIsNotValid()
    {
        // Arrange
        var newPost = new Abstractions.PostSave()
        {
            AuthorId = 1,
            Content = "Content5",
            Description = "Description5",
            Title = "Title5"
        };
        var modelError = new FluentValidation.Results.ValidationResult()
        {
            Errors = new List<FluentValidation.Results.ValidationFailure>()
            {
                new FluentValidation.Results.ValidationFailure() { ErrorMessage="errror" }
            }
        };
        _validatorMock.Setup(x => x.ValidateAsync(newPost, It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelError);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(newPost, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task CreateAsync_WithValidModel_ReturnsSuccess()
    {
        // Arrange
        var newPostId = 5;
        var newPost = new Abstractions.PostSave()
        {
            AuthorId = 1,
            Content = "Content5",
            Description = "Description5",
            Title = "Title5"
        };
        var modelOk = new FluentValidation.Results.ValidationResult()
        {
            Errors = new List<FluentValidation.Results.ValidationFailure>(),
        };
        _postRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<PostSave>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newPostId);
        _validatorMock.Setup(x => x.ValidateAsync(newPost, It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelOk);

        // Act
        var result = await _service.CreateAsync(newPost, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(newPostId, result);
    }
}
