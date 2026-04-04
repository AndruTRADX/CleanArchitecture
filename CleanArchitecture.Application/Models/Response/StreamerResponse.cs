using System;

namespace CleanArchitecture.Application.Models.Response;

public class StreamerResponse
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public virtual ICollection<VideoResponse> Videos { get; set; } = [];
}
