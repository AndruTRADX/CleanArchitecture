using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Directors;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Directors.Queries.GetPagedDirectors;

public class GetPagedDirectorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPagedDirectorsQuery, PagedResponse<DirectorResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResponse<DirectorResponse>> Handle(GetPagedDirectorsQuery request, CancellationToken cancellationToken)
    {
        var spec = new DirectorSpecification(request);
        var response = await _unitOfWork.Repository<Director>().GetAllWithSpec(spec);

        var specCount = new DirectorCountSpecification(request);
        var totalCount = await _unitOfWork.Repository<Director>().CountAsync(specCount);

        var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(request.PageSize)));

        var data = _mapper.Map<IReadOnlyList<Director>, IReadOnlyList<DirectorResponse>>(response);

        return new PagedResponse<DirectorResponse>
        {
            Count = totalCount,
            Data = data,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
        };
    }
}
