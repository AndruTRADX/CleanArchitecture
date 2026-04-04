using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence;

public class StreamerDbContextSeed
{
    public static async Task SeedAsync(StreamerDbContext context, ILoggerFactory loggerFactory)
    {
        if (context.Streamers != null && !context.Streamers.Any())
        {
            var logger = loggerFactory.CreateLogger<StreamerDbContextSeed>();

            context.Streamers.AddRange([
                new Streamer { CreatedBy = "andru", Name = "HBO", Url = "https://www.hbo.com" },
                new Streamer { CreatedBy = "andru", Name = "Prime Video", Url = "https://www.primevideo.com" },
                new Streamer { CreatedBy = "andru", Name = "Prime Video", Url = "https://www.primevideo.com" },
            ]);

            await context.SaveChangesAsync();

            logger.LogInformation("Inserted data from base seed {context}", nameof(StreamerDbContext));
        }
    }
}
