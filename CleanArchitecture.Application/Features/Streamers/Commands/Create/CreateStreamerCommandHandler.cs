using System;
using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Email;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.Create;

public class CreateStreamerCommandHandler(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IEmailService emailService, 
    ILogger<CreateStreamerCommandHandler> logger) : IRequestHandler<CreateStreamerCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<CreateStreamerCommandHandler> _logger = logger;


    public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
    {
        var data = _mapper.Map<Streamer>(request);
        _unitOfWork.Repository<Streamer>().AddEntity(data);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        try
        {
            var email = new Email
            {
                To = "adxcontacto@gmail.com",
                Body = $"La compañía de streamer ({data.Name}) con el ID ({data.Id}) se creó correctamente",
                Subject = $"Creación de compañía {data.Name}"
            };

            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error enviando el email de creación de streamer {data.Id}", ex);
        }

        return data.Id;
    }
}
