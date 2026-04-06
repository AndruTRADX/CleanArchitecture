using System;

namespace CleanArchitecture.Application.Models.Response;

public class ActorResponse
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
