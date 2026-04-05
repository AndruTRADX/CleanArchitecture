using CleanArchitecture.Application.Features.Directors.Commands.Create;
using CleanArchitecture.Application.Features.Directors.Queries.GetPagedDirectors;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<DirectorResponse>>> Create([FromQuery] GetPagedDirectorsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Create(CreateDirectorCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
