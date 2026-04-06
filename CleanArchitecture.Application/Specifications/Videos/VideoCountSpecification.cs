using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Videos;

public class VideoCountSpecification(VideoSpecificationParams specParams) : BaseSpecification<Video>(
    x =>
        (string.IsNullOrWhiteSpace(specParams.Search) || x.Name.Contains(specParams.Search)) &&
        (!specParams.DirectorId.HasValue || x.DirectorId.Equals(specParams.DirectorId)) && 
        (!specParams.StreamerId.HasValue || x.StreamerId.Equals(specParams.StreamerId))
    )
{
    
}