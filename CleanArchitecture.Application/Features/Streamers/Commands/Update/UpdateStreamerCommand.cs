using System;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Update;

public class UpdateStreamerCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; }  = string.Empty;
}
