using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Videos;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetPaged;

public class GetPagedVideosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPagedVideosQuery, PagedResponse<VideoResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResponse<VideoResponse>> Handle(GetPagedVideosQuery request, CancellationToken cancellationToken)
    {
        var spec = new VideoSpecification(request);
        var response = await _unitOfWork.Repository<Video>().GetAllWithSpec(spec);

        var specCount = new VideoCountSpecification(request);
        var totalCount = await _unitOfWork.Repository<Video>().CountAsync(specCount);

        var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(request.PageSize)));

        var data = _mapper.Map<IReadOnlyList<Video>, IReadOnlyList<VideoResponse>>(response);

        return new PagedResponse<VideoResponse>
        {
            Count = totalCount,
            Data = data,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
        };
    }
}
