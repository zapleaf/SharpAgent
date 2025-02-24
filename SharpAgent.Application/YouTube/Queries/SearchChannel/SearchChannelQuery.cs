using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.YouTube.Queries.SearchChannel;

public class SearchChannelQuery : IRequest<List<ChannelResponse>>
{
    public required string SearchTerm { get; set; }
}
