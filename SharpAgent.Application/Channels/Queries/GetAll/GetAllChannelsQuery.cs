using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.Channels.Queries.GetAll;

public class GetAllChannelsQuery : IRequest<List<ChannelDto>>
{
    public bool IncludeCategories { get; set; }
    public bool IncludeVideos { get; set; }
}