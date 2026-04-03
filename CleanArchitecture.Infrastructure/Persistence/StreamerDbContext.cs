using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class StreamerDbContext(DbContextOptions<StreamerDbContext> options) : DbContext(options)
{

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "system";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "system";
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Streamer>()
            .HasMany(m => m.Videos)
            .WithOne(m => m.Streamer)
            .HasForeignKey(m => m.StreamerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Director>()
            .HasMany(v => v.Videos)
            .WithOne(d => d.Director)
            .HasForeignKey(d => d.DirectorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Video>()
            .HasMany(a => a.Actors)
            .WithMany(v => v.Videos)
            .UsingEntity<VideoActor>(
                j => j.HasOne(p => p.Actor).WithMany(p => p.VideoActors).HasForeignKey(p => p.ActorId),
                j => j.HasOne(p => p.Video).WithMany(p => p.VideoActors).HasForeignKey(p => p.VideoId),
                j => j.HasKey(t => new { t.ActorId, t.VideoId })
            );
        
        modelBuilder.Entity<VideoActor>().Ignore(va => va.Id); // This attribute will not be mapped when migrating
    }


    public DbSet<Streamer>? Streamers { get; set; }

    public DbSet<Video>? Videos { get; set; }

    public DbSet<Actor>? Actors { get; set; }

    public DbSet<Director>? Directores { get; set; }

}