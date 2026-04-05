using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Specifications.Streamers;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Queries.GetByUrl;

public class GetStreamersByUrlQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetStreamersByUrlQuery, List<StreamerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<List<StreamerResponse>> Handle(GetStreamersByUrlQuery request, CancellationToken cancellationToken)
    {
        var spec = new StreamersWithVideosSpecification(request.Url);
        var response = await _unitOfWork.Repository<Streamer>().GetAllWithSpec(spec);

        return _mapper.Map<List<StreamerResponse>>(response);
    }
}
