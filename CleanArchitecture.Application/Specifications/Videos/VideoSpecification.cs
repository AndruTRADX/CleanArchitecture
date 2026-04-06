using System;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Videos;

public class VideoSpecification: BaseSpecification<Video>
{
    public VideoSpecification(VideoSpecificationParams specParams) : base(
        x =>
            (string.IsNullOrWhiteSpace(specParams.Search) || x.Name.Contains(specParams.Search)) &&
            (!specParams.DirectorId.HasValue || x.DirectorId.Equals(specParams.DirectorId)) && 
            (!specParams.StreamerId.HasValue || x.StreamerId.Equals(specParams.StreamerId))
    )
    {
        AddInclude(p => p.Director!);
        AddInclude(p => p.Streamer!);
        AddInclude(p => p.Actors!);

        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        if (!string.IsNullOrWhiteSpace(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "name":
                    AddOrderBy(p => p.Name);
                    break;
                case "nameDesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                case "createdDate":
                    AddOrderBy(p => p.CreatedDate!);
                    break;
                case "createdDateDesc":
                    AddOrderByDescending(p => p.CreatedDate!);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
}