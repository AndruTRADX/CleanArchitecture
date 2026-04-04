using System;
using CleanArchitecture.Application.Models.Response;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUsername;

public class GetStreamersByUsernameQuery : IRequest<List<StreamerResponse>>
{
    public string Username { get; set; } = string.Empty;
}
