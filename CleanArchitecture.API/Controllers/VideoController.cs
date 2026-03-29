using CleanArchitecture.Application.Features.Videos.Queries.GetVideos;
using CleanArchitecture.Application.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<VideoResponse>>> GetVideosByUserName(string username)
        {
            return Ok(await _mediator.Send(new GetVideosQuery { UserName = username }));
        }
    }
}
