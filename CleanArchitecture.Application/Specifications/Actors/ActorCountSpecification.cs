using System;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Actors;

public class ActorCountSpecification(ActorSpecificationParams specification) : BaseSpecification<Actor>(
    x => 
        string.IsNullOrWhiteSpace(specification.Search) || x.Name.Contains(specification.Search)
    )
{
    
}
