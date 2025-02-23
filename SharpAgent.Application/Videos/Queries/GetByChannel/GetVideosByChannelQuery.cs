using MediatR;
using SharpAgent.Application.Videos.Common;

namespace SharpAgent.Application.Videos.Queries.GetByChannel;

public class GetVideosByChannelQuery : IRequest<List<VideoDto>>
{
    public Guid ChannelId { get; set; }
}
