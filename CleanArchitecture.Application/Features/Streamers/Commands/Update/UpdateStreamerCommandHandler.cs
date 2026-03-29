using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Update;

public class UpdateStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper) : IRequestHandler<UpdateStreamerCommand, Unit>
{
    private readonly IStreamerRepository _streamerRepository = streamerRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _streamerRepository.GetByIdAsync(request.Id) 
            ?? throw new NotFoundException("Streamer", request.Id);

        var data = _mapper.Map(request, entity);

        await _streamerRepository.AddAsync(data);

        return Unit.Value;
    }
}
