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
    IStreamerRepository streamerRepository, 
    IMapper mapper, 
    IEmailService emailService, 
    ILogger<CreateStreamerCommandHandler> logger) : IRequestHandler<CreateStreamerCommand, int>
{
    private readonly IStreamerRepository _streamerRepository = streamerRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<CreateStreamerCommandHandler> _logger = logger;


    public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
    {
        var data = _mapper.Map<Streamer>(request);
        var response = await _streamerRepository.AddAsync(data);

        try
        {
            var email = new Email
            {
                To = "adxcontacto@gmail.com",
                Body = $"La compañía de streamer ({response.Name}) con el ID ({response.Id}) se creó correctamente",
                Subject = $"Creación de compañía {response.Name}"
            };

            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error enviando el email de creación de streamer {response.Id}", ex);
        }

        return response.Id;
    }
}
