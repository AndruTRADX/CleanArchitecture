using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Create;

public class CreateStreamerCommandValidator : AbstractValidator<CreateStreamerCommand>
{
    public CreateStreamerCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(50)
            .WithMessage("El nombre no puede exceder los 50 caracteres");

        RuleFor(p => p.Url)
            .NotEmpty()
            .WithMessage("La URL es obligatoria");
    }
}
