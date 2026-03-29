using System;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class VideoRepository(StreamerDbContext dbContext) : BaseRepository<Video>(dbContext), IVideoRepository
{
    public async Task<Video?> GetVideoByName(string videoName)
    {
        var video = await _dbContext.Videos!.Where(v => v.Name == videoName).FirstOrDefaultAsync();
        return video;
    }

    public async Task<IEnumerable<Video>> GetVideoByUserName(string userName)
    {
        var video = await _dbContext.Videos!.Where(v => v.CreatedBy == userName).ToListAsync();
        return video;
    }
}
