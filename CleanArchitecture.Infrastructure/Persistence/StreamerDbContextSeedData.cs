using System;
using System.Net.Http.Headers;
using System.Text.Json;
using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence;

public class StreamerDbContextSeedData
{
    public static async Task LoadDataAsync(StreamerDbContext context, ILoggerFactory loggerFactory)
    {
        var videos = new List<Video>();

        try
        {
            if (!context.Directors.Any())
            {
                var directorData = File.ReadAllText("../CleanArchitecture.Infrastructure/Data/director.json");

                var directors = JsonSerializer.Deserialize<List<Director>>(directorData);

                if (directors != null && directors.Count > 0)
                {
                    await context.Directors.AddRangeAsync(directors);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Videos.Any())
            {
                var videosData = File.ReadAllText("../CleanArchitecture.Infrastructure/Data/video.json");

                videos = JsonSerializer.Deserialize<List<Video>>(videosData);

                if (videos != null && videos.Count > 0)
                {
                    await SetRandomDirectorVideoAsync(videos, context);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Actors.Any())
            {
                var actorData = File.ReadAllText("../CleanArchitecture.Infrastructure/Data/actor.json");

                var actors = JsonSerializer.Deserialize<List<Actor>>(actorData);

                if (actors != null && actors.Count > 0)
                    await context.Actors.AddRangeAsync(actors);

                if (videos != null && videos.Count > 0)
                    await context.VideoActors.AddRangeAsync(GetVideoActorAsync(videos));

                await context.SaveChangesAsync();
            }
        }
        catch (System.Exception ex)
        {
            var logger = loggerFactory.CreateLogger<StreamerDbContextSeedData>();

            logger.LogError(ex.Message);
        }
    }

    private static async Task SetRandomDirectorVideoAsync(List<Video> videos, StreamerDbContext context)
    {
        var random = new Random();

        foreach (var video in videos)
        {
            video.DirectorId = random.Next(1, 99);
        }

        await context.Videos.AddRangeAsync(videos);
    }

    private static List<VideoActor> GetVideoActorAsync(List<Video> videos)
    {
        var videoActors = new List<VideoActor>();
        var random = new Random();

        foreach (var video in videos)
        {
            var videoActor = new VideoActor
            {
                VideoId = video.Id,
                ActorId = random.Next(1, 99),
            };

            videoActors.Add(videoActor);
        }

        return videoActors;
    }
}
