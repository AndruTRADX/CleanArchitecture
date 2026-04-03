using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain;

public class Video : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;
    public int StreamerId { get; set; }
    public int DirectorId { get; set; }

    public virtual Streamer? Streamer { get; set; }
    public virtual Director? Director { get; set; }
    public virtual ICollection<Actor> Actors { get; set; } = [];
    public virtual ICollection<VideoActor> VideoActors { get; set; } = [];
}