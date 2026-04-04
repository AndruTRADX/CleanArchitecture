using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Update;

public class UpdateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateStreamerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<Streamer>().GetFirstAsync(p => p.Id == request.Id)
            ?? throw new NotFoundException("Streamer", request.Id);

        var data = _mapper.Map(request, entity);

        _unitOfWork.Repository<Streamer>().UpdateEntity(data);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
