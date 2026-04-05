using CleanArchitecture.Application.Features.Streamers.Commands.Create;
using CleanArchitecture.Application.Features.Streamers.Commands.Delete;
using CleanArchitecture.Application.Features.Streamers.Commands.Update;
using CleanArchitecture.Application.Features.Streamers.Queries.GetByUrl;
using CleanArchitecture.Application.Features.Streamers.Queries.GetByUsername;
using CleanArchitecture.Application.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<IEnumerable<StreamerResponse>>> GetByUsername(string username)
        {
            return await _mediator.Send(new GetStreamersByUsernameQuery { Username = username });
        }

        // Bad practice though, this could be just one method, but it's just to do it with the specification pattern
        [HttpGet("ByUrl/{url}")]
        public async Task<ActionResult<IEnumerable<StreamerResponse>>> GetByUrl(string url)
        {
            return await _mediator.Send(new GetStreamersByUrlQuery { Url = url });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<int>> Create(CreateStreamerCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Unit>> Update(UpdateStreamerCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            await _mediator.Send(new DeleteStreamerCommand { Id = id });
            return NoContent();
        }
    }
}
