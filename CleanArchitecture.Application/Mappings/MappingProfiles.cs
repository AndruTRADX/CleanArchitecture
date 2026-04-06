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
        CreateMap<Video, VideoResponse>()
            .ForMember(src => src.StreamerName, 
                opt => opt.MapFrom(dest => dest.Streamer == null ? string.Empty : (dest.Streamer.Name ?? string.Empty)))
            .ForMember(src => src.DirectorFullName,
                opt => opt.MapFrom(dest => dest.Director == null 
                    ? string.Empty 
                    : $"{dest.Director.Name ?? string.Empty} {dest.Director.LastName ?? string.Empty}"));

        CreateMap<Streamer, StreamerResponse>();
        CreateMap<CreateStreamerCommand, Streamer>();
        CreateMap<UpdateStreamerCommand, Streamer>();

        CreateMap<Director, DirectorResponse>();
        CreateMap<CreateDirectorCommand, Director>();

        CreateMap<Actor, ActorResponse>();
    }
}
