using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.Channels.Queries.GetAll;

public class GetAllChannelsQuery : IRequest<List<ChannelResponse>>
{
    public bool IncludeCategories { get; set; }
    public bool IncludeVideos { get; set; }
}