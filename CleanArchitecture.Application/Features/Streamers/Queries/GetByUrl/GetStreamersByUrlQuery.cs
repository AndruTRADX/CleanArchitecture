using System;
using CleanArchitecture.Application.Models.Response;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUrl;

public class GetStreamersByUrlQuery : IRequest<List<StreamerResponse>>
{
    public string Url { get; set; } = string.Empty;
}
