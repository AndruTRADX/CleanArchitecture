using CleanArchitecture.Application.Features.Actors.Queries.GetPaged;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ActorResponse>>> GetPaged([FromQuery] GetPagedActorsQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
