using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.YouTube.Queries.GetChannelStats;

public class GetChannelStatsQuery : IRequest<Channel>
{
    public required string YoutubeId { get; set; }
}
