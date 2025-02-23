using MediatR;

namespace SharpAgent.Application.YouTube.Commands.SaveChannel;

public class SaveChannelCommand : IRequest<Guid>
{
    public required string YTId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailURL { get; set; }
    public DateTime PublishedAt { get; set; }
}
