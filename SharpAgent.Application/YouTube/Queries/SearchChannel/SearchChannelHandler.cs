using SharpAgent.Application.IServices;
using SharpAgent.Application.Channels.Common;
using MediatR;

namespace SharpAgent.Application.YouTube.Queries.SearchChannel;

public class SearchChannelHandler : IRequestHandler<SearchChannelQuery, List<ChannelDto>>
{
    private readonly IYouTubeApiService _youTubeService;

    public SearchChannelHandler(IYouTubeApiService youTubeService)
    {
        _youTubeService = youTubeService;
    }

    public async Task<List<ChannelDto>> Handle(SearchChannelQuery request, CancellationToken cancellationToken)
    {
        return await _youTubeService.ChannelSearch(request.SearchTerm);
    }
}
