using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUsername;

public class GetStreamersByUsernameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetStreamersByUsernameQuery, List<StreamerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<StreamerResponse>> Handle(GetStreamersByUsernameQuery request, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.Repository<Streamer>().GetAsync(
            predicate: p => p.CreatedBy == request.Username,
            orderBy: o => o.OrderBy(x => x.CreatedDate),
            includes: [
                x => x.Videos
            ],
            disableTracking: true
        );

        var data = _mapper.Map<List<StreamerResponse>>(response);

        return data;
    }
}
