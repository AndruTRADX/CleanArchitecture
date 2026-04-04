using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUsername;

public class GetStreamersByUsernameQueryValidator : AbstractValidator<GetStreamersByUsernameQuery>
{
    public GetStreamersByUsernameQueryValidator()
    {
        RuleFor(v => v.Username)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Username is required and must have at least 3 characters");
    }
}
