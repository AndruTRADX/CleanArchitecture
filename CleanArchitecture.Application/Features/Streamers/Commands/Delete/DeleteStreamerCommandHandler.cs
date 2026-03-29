using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Delete;

public class DeleteStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper) : IRequestHandler<DeleteStreamerCommand, Unit>
{
    private readonly IStreamerRepository _streamerRepository = streamerRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
    {
        var response = await _streamerRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("Streamer", request.Id);

        await _streamerRepository.DeleteAsync(response);

        return Unit.Value;
    }
}
