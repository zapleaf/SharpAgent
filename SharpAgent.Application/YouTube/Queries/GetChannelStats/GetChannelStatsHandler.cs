using SharpAgent.Application.IServices;
using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.YouTube.Queries.GetChannelStats;

public class GetChannelStatsHandler : IRequestHandler<GetChannelStatsQuery, Channel>
{
    private readonly IYouTubeApiService _youTubeService;

    public GetChannelStatsHandler(IYouTubeApiService youTubeService)
    {
        _youTubeService = youTubeService;
    }

    public async Task<Channel> Handle(GetChannelStatsQuery request, CancellationToken cancellationToken)
    {
        return await _youTubeService.GetChannelStats(request.YoutubeId);
    }
}
