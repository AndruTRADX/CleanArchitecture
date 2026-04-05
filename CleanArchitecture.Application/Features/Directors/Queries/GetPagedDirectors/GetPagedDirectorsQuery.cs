using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Directors;
using MediatR;

namespace CleanArchitecture.Application.Features.Directors.Queries.GetPagedDirectors;

public class GetPagedDirectorsQuery : DirectorSpecificationParams, IRequest<PagedResponse<DirectorResponse>>
{

}
