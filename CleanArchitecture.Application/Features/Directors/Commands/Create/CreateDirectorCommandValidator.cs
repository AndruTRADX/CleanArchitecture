using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Directors.Commands.Create;

public class CreateDirectorCommandValidator : AbstractValidator<CreateDirectorCommand>
{
    public CreateDirectorCommandValidator()
    {
        RuleFor(prop => prop.Name)
            .NotNull()
            .MinimumLength(3)
            .WithMessage("Name is required and must have at least 3 characters");

        RuleFor(prop => prop.LastName)
            .NotNull()
            .MinimumLength(3)
            .WithMessage("LastName is required and must have at least 3 characters");

        RuleFor(prop => prop.VideoId)
            .GreaterThan(0)
            .WithMessage("VideoId is required");
    }
}
