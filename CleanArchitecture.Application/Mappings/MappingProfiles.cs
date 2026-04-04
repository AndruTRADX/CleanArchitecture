using System;
using AutoMapper;
using CleanArchitecture.Application.Features.Directors.Commands.Create;
using CleanArchitecture.Application.Features.Streamers.Commands.Create;
using CleanArchitecture.Application.Features.Streamers.Commands.Update;
using CleanArchitecture.Application.Models.Response;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Video, VideoResponse>();

        CreateMap<Streamer, StreamerResponse>();
        CreateMap<CreateStreamerCommand, Streamer>();
        CreateMap<UpdateStreamerCommand, Streamer>();

        CreateMap<CreateDirectorCommand, Director>();
    }
}
