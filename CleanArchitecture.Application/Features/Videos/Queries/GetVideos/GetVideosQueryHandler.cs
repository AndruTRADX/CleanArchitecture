using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideos;

public class GetVideosQueryHandler(IVideoRepository videoRepository, IMapper mapper) : IRequestHandler<GetVideosQuery, List<VideoResponse>>
{
    private readonly IVideoRepository _videoRepository = videoRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<VideoResponse>> Handle(GetVideosQuery request, CancellationToken cancellationToken)
    {
        var response = await _videoRepository.GetVideoByUserName(request.UserName);
        
        return _mapper.Map<List<VideoResponse>>(response);
    }
}
