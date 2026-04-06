using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Actors;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Actors.Queries.GetPaged;

public class GetPagedActorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPagedActorsQuery, PagedResponse<ActorResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResponse<ActorResponse>> Handle(GetPagedActorsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ActorSpecification(request);
        var response = await _unitOfWork.Repository<Actor>().GetAllWithSpec(spec);

        var specCount = new ActorCountSpecification(request);
        var totalCount = await _unitOfWork.Repository<Actor>().CountAsync(specCount);

        var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(request.PageSize)));

        var data = _mapper.Map<IReadOnlyList<Actor>, IReadOnlyList<ActorResponse>>(response);

        return new PagedResponse<ActorResponse>
        {
            Count = totalCount,
            Data = data,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
        };
    }
}
