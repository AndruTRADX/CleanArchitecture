using CleanArchitecture.Application.Features.Directors.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Create(CreateDirectorCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
