using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUrl;

public class GetStreamersByUrlQueryValidator : AbstractValidator<GetStreamersByUrlQuery>
{
    public GetStreamersByUrlQueryValidator()
    {
        RuleFor(v => v.Url)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Url is required and must have at least 3 characters");
    }
}
