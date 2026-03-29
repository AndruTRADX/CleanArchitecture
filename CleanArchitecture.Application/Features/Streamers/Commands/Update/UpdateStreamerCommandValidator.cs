using System;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Update;

public class UpdateStreamerCommandValidator : AbstractValidator<UpdateStreamerCommand>
{
    public UpdateStreamerCommandValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("El id es obligatorio y no puede ser 0");

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
