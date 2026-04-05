using System;

namespace CleanArchitecture.Application.Models.Response.Common;

public class PagedResponse<T> where T : class
{
    public int Count { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public IReadOnlyList<T> Data { get; set; } = [];
}
