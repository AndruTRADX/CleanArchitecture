using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;

namespace CleanArchitecture.Application.Features.Directors.Commands.Create;

public class CreateDirectorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDirectorCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<int> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Director>(request);

        _unitOfWork.Repository<Director>().AddEntity(entity);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (result <= 0) throw new BadRequestException("Record couldn't be inserted.");

        return entity.Id;
    }
}
