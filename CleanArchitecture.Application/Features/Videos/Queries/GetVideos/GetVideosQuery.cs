using CleanArchitecture.Application.Models.Response;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideos;

public class GetVideosQuery : IRequest<List<VideoResponse>>
{
    public string UserName { get; set; } = string.Empty;
}
