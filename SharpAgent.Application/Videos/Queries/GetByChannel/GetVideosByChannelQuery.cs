using MediatR;
using SharpAgent.Application.Videos.Common;

namespace SharpAgent.Application.Videos.Queries.GetByChannel;

public class GetVideosByChannelQuery : IRequest<List<VideoResponse>>
{
    public Guid ChannelId { get; set; }
}
