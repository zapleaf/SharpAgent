using SharpAgent.Application.IServices;
using SharpAgent.Application.Channels.Common;
using MediatR;

namespace SharpAgent.Application.YouTube.Queries.SearchChannel;

public class SearchChannelHandler : IRequestHandler<SearchChannelQuery, List<ChannelResponse>>
{
    private readonly IYouTubeApiService _youTubeService;

    public SearchChannelHandler(IYouTubeApiService youTubeService)
    {
        _youTubeService = youTubeService;
    }

    public async Task<List<ChannelResponse>> Handle(SearchChannelQuery request, CancellationToken cancellationToken)
    {
        return await _youTubeService.ChannelSearch(request.SearchTerm);
    }
}
