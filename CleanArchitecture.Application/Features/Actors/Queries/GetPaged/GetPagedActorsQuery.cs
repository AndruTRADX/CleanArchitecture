using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using CleanArchitecture.Application.Specifications.Actors;
using MediatR;

namespace CleanArchitecture.Application.Features.Actors.Queries.GetPaged;

public class GetPagedActorsQuery : ActorSpecificationParams, IRequest<PagedResponse<ActorResponse>>
{

}
