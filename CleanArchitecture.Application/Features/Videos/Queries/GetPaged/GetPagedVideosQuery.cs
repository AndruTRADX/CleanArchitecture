using System;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Videos;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetPaged;

public class GetPagedVideosQuery : VideoSpecificationParams, IRequest<PagedResponse<VideoResponse>>
{

}
