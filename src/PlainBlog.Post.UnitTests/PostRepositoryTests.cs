using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using PlainBlog.Post.Repository;
using PlainBlog.Store;
using Xunit;

namespace PlainBlog.Post.UnitTests;

[Trait("Category", nameof(PostRepositoryTests))]
public class PostRepositoryTests
{
    private readonly IPostRepository _repository;
    private readonly Mock<IServiceProvider> _serviceProviderMock = new();
    private readonly Mock<AbstractPlainBlogContext> _contextMock;

    private readonly List<PlainBlog.Store.Entities.Post> _posts;

    public PostRepositoryTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_serviceProviderMock.Object);
        serviceCollection.BuildServiceProvider();

        var dbContextOptions = (new DbContextOptionsBuilder<AbstractPlainBlogContext>()).Options;
        _contextMock = new Mock<AbstractPlainBlogContext>(dbContextOptions, _serviceProviderMock.Object);
        _repository = new PostRepository(_contextMock.Object);

        _posts = new List<Store.Entities.Post>()
        {
            new (){ Id = 1, Title ="Title1", Description="Description1", Content="Content1", AuthorId= 1 },
            new (){ Id = 2, Title ="Title2", Description="Description2", Content="Content2", AuthorId= 1 },
            new (){ Id = 3, Title ="Title3", Description="Description3", Content="Content3", AuthorId= 1 },
            new (){ Id = 4, Title ="Title4", Description="Description4", Content="Content4", AuthorId= 1 },
        };
    }

    [Fact]
    public async Task GetAsync_Returns_Correct_Posts()
    {
        // Arrange
        _contextMock.Setup<DbSet<Store.Entities.Post>>(x => x.Posts)
                    .ReturnsDbSet(_posts);

        // Act
        var result = await _repository.GetAsync(It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count());

        var r0 = result.ToList()[0];
        Assert.Equal(_posts.ToList()[0].Title, r0.Title);
        Assert.Equal(_posts.ToList()[0].Id, r0.Id);

        var r3 = result.ToList()[3];
        Assert.Equal(_posts.ToList()[3].Title, r3.Title);
        Assert.Equal(_posts.ToList()[3].Id, r3.Id);
    }

    [Fact]
    public async Task GetAsync_Returns_Correct_Post()
    {
        // Arrange
        var postId = 3;
        _contextMock.Setup<DbSet<Store.Entities.Post>>(x => x.Posts)
                    .ReturnsDbSet(_posts);

        // Act
        var result = await _repository.GetAsync(postId, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_posts.ToList()[2].Title, result.Title);
        Assert.Equal(_posts.ToList()[2].Id, result.Id);
    }

    [Fact]
    public async Task GetAsync_WhenPostNotFound_ShouldReturnNull()
    {
        // Arrange
        var postId = 30;
        _contextMock.Setup<DbSet<Store.Entities.Post>>(x => x.Posts)
                    .ReturnsDbSet(_posts);

        // Act
        var result = await _repository.GetAsync(postId, It.IsAny<CancellationToken>());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePostAndReturnId()
    {
        //Arrange
        var model = new Abstractions.PostSave()
        {
            AuthorId = 1,
            Content = "Content5",
            Description = "Description5",
            Title = "Title5"
        };
        var generatedId = 5;
        _contextMock.Setup<DbSet<Store.Entities.Post>>(x => x.Posts)
                    .ReturnsDbSet(_posts);
        _contextMock.Setup(context => context.Posts.AddAsync(It.IsAny<Store.Entities.Post>(), It.IsAny<CancellationToken>()))
                   .Callback<Store.Entities.Post, CancellationToken>((post, _) =>
                   {
                       post.Id = generatedId;
                   })
                   .Returns<Store.Entities.Post, CancellationToken>((e, c) =>
                   {
                       var stateManagerMock = new Mock<IStateManager>();
                       var entityTypeMock = new Mock<IRuntimeEntityType>();
                       entityTypeMock
                           .SetupGet(_ => _.EmptyShadowValuesFactory)
                           .Returns(() => new Mock<ISnapshot>().Object);
                       entityTypeMock
                           .Setup(_ => _.GetProperties())
                           .Returns(Enumerable.Empty<IProperty>());
                       var internalEntity = new InternalEntityEntry(stateManagerMock.Object,
                           entityTypeMock.Object, e);
                       var entry = new EntityEntry<Store.Entities.Post>(internalEntity);
                       return ValueTask.FromResult(entry);
                   });

        // Act
        var result = await _repository.CreateAsync(model, It.IsAny<CancellationToken>());

        //Assert
        Assert.Equal(generatedId, result);
        _contextMock.Verify(m => m.Posts.AddAsync(It.IsAny<Store.Entities.Post>(), It.IsAny<CancellationToken>()), Times.Once);
        _contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
