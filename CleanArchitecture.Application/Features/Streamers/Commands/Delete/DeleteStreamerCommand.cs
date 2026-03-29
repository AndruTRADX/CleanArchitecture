using System;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Delete;

public class DeleteStreamerCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
