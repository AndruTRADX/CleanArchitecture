using CleanArchitecture.Application.Features.Videos.Queries.GetPaged;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideos;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Application.Models.Response.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VideoResponse>>> GetVideosByUserName(string username)
        {
            return Ok(await _mediator.Send(new GetVideosQuery { UserName = username }));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<VideoResponse>>> GetPaged([FromQuery] GetPagedVideosQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
