using MediatR;

namespace SharpAgent.Application.YouTube.Commands.SaveVideos;

public class SaveChannelVideosCommand : IRequest<int>
{
    public required string ChannelYTId { get; set; }
    public Guid ChannelId { get; set; }
    public DateTime? LastCheckDate { get; set; }
}