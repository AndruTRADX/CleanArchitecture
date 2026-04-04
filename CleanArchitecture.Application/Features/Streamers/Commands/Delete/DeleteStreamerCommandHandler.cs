using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Delete;

public class DeleteStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteStreamerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.Repository<Streamer>().GetFirstAsync(p => p.Id == request.Id)
            ?? throw new NotFoundException("Streamer", request.Id);

        _unitOfWork.Repository<Streamer>().DeleteEntity(response);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
