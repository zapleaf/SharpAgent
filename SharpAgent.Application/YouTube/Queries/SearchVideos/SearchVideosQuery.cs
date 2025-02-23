using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.YouTube.Queries.SearchVideos;

public class SearchVideosQuery : IRequest<List<Video>>
{
    public required string SearchTerm { get; set; }
    public DateTime? StartDate { get; set; }
}
