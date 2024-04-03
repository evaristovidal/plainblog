using FluentValidation;
using Microsoft.Extensions.Localization;
using PlainBlog.Post.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainBlog.Post.Validator;

public class PostSaveValidator : AbstractValidator<PostSave>
{
    public PostSaveValidator(IStringLocalizer<i18n.Server> stringLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage("WorkflowTask cannot be null");

        RuleFor(m => m.AuthorId)
           .GreaterThan(0)
               .WithMessage(stringLocalizer[i18n.Server.PropertyNameShouldNotBeEmpty])
               .WithErrorCode(nameof(i18n.Server.PropertyNameShouldNotBeEmpty));
        
        RuleFor(m => m.Title)
            .Length(1, 250)
                .WithMessage(stringLocalizer[i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols])
                .WithErrorCode(nameof(i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols));

        RuleFor(m => m.Description)
           .Length(1, 250)
               .WithMessage(stringLocalizer[i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols])
               .WithErrorCode(nameof(i18n.Server.PropertyNameLengthShouldBeBetween1And255Symbols));

        RuleFor(m => m.Content)
            .MinimumLength(1)
                .WithMessage(stringLocalizer[i18n.Server.PropertyNameShouldNotBeEmpty])
                .WithErrorCode(nameof(i18n.Server.PropertyNameShouldNotBeEmpty));
    }
}
