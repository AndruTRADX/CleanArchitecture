using System;

namespace CleanArchitecture.Application.Models.Response;

public class VideoResponse
{
    public string Name { get; set; } = string.Empty;
    public int StreamerId { get; set; }
    public string StreamerName { get; set; } = string.Empty;
    public int DirectorId { get; set; }
    public string DirectorFullName { get; set; } = string.Empty;
    
    public virtual ICollection<ActorResponse> Actors { get; set; } = [];
}
