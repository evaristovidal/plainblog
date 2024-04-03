using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Post.Abstractions;
using PlainBlog.Post.Extensions;
using Xunit;

namespace PlainBlog.Post.UnitTests;

[Trait("Category", nameof(PostSaveValidatorTests))]
public class PostSaveValidatorTests
{
    private readonly IValidator<PostSave> _validator;

    public PostSaveValidatorTests()
    {
        var services = new ServiceCollection();

        services.AddLocalization();
        services.AddLogging();
        services.AddPostCore();

        var serviceProvider = services.BuildServiceProvider();
        _validator = serviceProvider.GetService<IValidator<PostSave>>();
    }

    [Fact]
    public void PostSave_ValidModel_ReturnsSuccess()
    {
        // Arrange
        var postSave = new Abstractions.PostSave()
        {
            AuthorId = 1,
            Content = "Content5",
            Description = "Description5",
            Title = "Title5"
        };

        // Act
        var result = _validator.Validate(postSave);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void PostSave_WithMissingContent_ReturnsFailure()
    {
        // Arrange
        var postSave = new Abstractions.PostSave()
        {
            AuthorId = 1,
            Content = "",
            Description = "",
            Title = ""
        };

        // Act
        var result = _validator.Validate(postSave);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(result.Errors, failure => failure.PropertyName == "Title" && failure.ErrorCode == nameof(i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols));
        Assert.Contains(result.Errors, failure => failure.PropertyName == "Description" && failure.ErrorCode == nameof(i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols));
        Assert.Contains(result.Errors, failure => failure.PropertyName == "Content" && failure.ErrorCode == nameof(i18n.Server.PropertyNameShouldNotBeEmpty));
    }
}
