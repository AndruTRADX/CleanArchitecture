using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain;

public class VideoActor : BaseDomainModel
{
    public int VideoId { get; set; }
    public int ActorId { get; set; }

    public virtual Video? Video { get; set; }
    public virtual Actor? Actor { get; set; }
}